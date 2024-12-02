using AutoMapper;
using GRSMU.Bot.Common.Extensions;
using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Logic.Features.Schedule.Dtos;
using GRSMU.Bot.Logic.Features.Schedule.Services.Interfaces;
using GRSMU.Bot.Logic.Features.Users.Dtos;
using GRSMU.Bot.Logic.Immutable;

namespace GRSMU.Bot.Logic.Features.Schedule.Queries.GetByDate;

public class GetScheduleByDateQueryHandler : IQueryHandler<GetScheduleByDateQuery, ScheduleDayDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IScheduleService _scheduleService;
    private readonly IMapper _mapper;

    public GetScheduleByDateQueryHandler(
        IUserRepository userRepository, 
        IScheduleService scheduleService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _scheduleService = scheduleService;
        _mapper = mapper;
    }

    public async Task<ExecutionResult<ScheduleDayDto>> Handle(GetScheduleByDateQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);

        if (user is null)
        {
            return ExecutionResult<ScheduleDayDto>.Failure(Error.NotFound(ErrorResourceKeys.UserNotFound));
        }

        var userDto = _mapper.Map<UserDto>(user);
        var scheduleDaySearchResult = await _scheduleService.GetScheduleByDateAsync(PrepareDate(request.Date), userDto, request.ForceRefresh);

        if (scheduleDaySearchResult.HasErrors)
        {
            return ExecutionResult<ScheduleDayDto>.Failure(scheduleDaySearchResult.Error!);
        }

        return ExecutionResult.Success(scheduleDaySearchResult.Data);
    }

    private DateTime PrepareDate(DateTime date) => date.StartOfDay();
}