using AutoMapper;
using GRSMU.Bot.Common.Extensions;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Data.TimeTables.Contracts;
using GRSMU.Bot.Data.TimeTables.Contracts.Filters;
using GRSMU.Bot.Data.TimeTables.Documents;
using GRSMU.Bot.Logic.Features.Schedule.Dtos;
using GRSMU.Bot.Logic.Features.Schedule.Services.Interfaces;
using GRSMU.Bot.Logic.Features.Users.Dtos;
using GRSMU.Bot.Logic.Immutable;

namespace GRSMU.Bot.Logic.Features.Schedule.Services;

public class ScheduleService : IScheduleService
{
    private readonly IScheduleProvider _scheduleProvider;
    private readonly ITimeTableRepository _timeTableRepository;
    private readonly IMapper _mapper;

    public ScheduleService(
        IScheduleProvider scheduleProvider,
        ITimeTableRepository timeTableRepository,
        IMapper mapper)
    {
        _scheduleProvider = scheduleProvider;
        _timeTableRepository = timeTableRepository;
        _mapper = mapper;
    }

    public async Task<ExecutionResult<ScheduleDayDto>> GetScheduleByDateAsync(DateTime date, UserDto userDto, bool forceRefresh)
    {
        if (!userDto.IsStudentCardRegistered)
        {
            return ExecutionResult<ScheduleDayDto>.Failure(Error.Forbidden(ErrorResourceKeys.StudentCardIsNotRegistered));
        }

        var documents = await _timeTableRepository.GetDocuments(new TimeTableFilter
        {
            Date = date,
            GroupId = userDto.GroupId,
        });

        var scheduleDocument = documents.FirstOrDefault();

        if (scheduleDocument is null || 
            (forceRefresh && scheduleDocument.CreatedDate.HasValue && (DateTime.UtcNow - scheduleDocument.CreatedDate.Value).TotalMinutes > 30))
        {
            var week = date.StartOfWeek();

            var weekSchedule = await _scheduleProvider.GetScheduleByWeekAsync(week, userDto.FacultyId, userDto.CourseId,
                userDto.GroupId);

            if (!weekSchedule.Any())
            {
                return ExecutionResult<ScheduleDayDto>.Failure(Error.NotFound(ErrorResourceKeys.ScheduleNotFound));
            }

            documents = _mapper.Map<List<TimeTableDocument>>(weekSchedule);
            documents.ForEach(d =>
            {
                d.Week = DateTime.SpecifyKind(week, DateTimeKind.Utc);
                d.GroupId = userDto.GroupId;
            });

            await _timeTableRepository.UpsertManyAsync(documents, userDto.GroupId, week);

            scheduleDocument = documents.FirstOrDefault(x => x.Date >= date.StartOfDay() && x.Date <= date.EndOfDay());
        }

        if (scheduleDocument is null)
        {
            return ExecutionResult<ScheduleDayDto>.Failure(Error.NotFound(ErrorResourceKeys.ScheduleNotFound));
        }

        var scheduleDayDto = new ScheduleDayDto(
            scheduleDocument.Day,
            scheduleDocument.Date,
            scheduleDocument.Week,
            _mapper.Map<List<ScheduleItemDto>>(scheduleDocument.Lines ?? []));

        return ExecutionResult.Success(scheduleDayDto);
    }

    public Task<ExecutionResult<ScheduleDayDto>> GetNearestScheduleAsync(UserDto userDto, bool forceRefresh)
    {
        // TODO:Fix ASAP: This solution not covered cases, such as:
        // - Weekend on the working day
        // - It possible to have lectures in Saturday

        var today = DateTime.Today;

        if (today.DayOfWeek is DayOfWeek.Saturday)
        {
            today = today.AddDays(2);
        }
        else if (today.DayOfWeek is DayOfWeek.Sunday)
        {
            today = today.AddDays(1);
        }

        return GetScheduleByDateAsync(today, userDto, forceRefresh);
    }
}