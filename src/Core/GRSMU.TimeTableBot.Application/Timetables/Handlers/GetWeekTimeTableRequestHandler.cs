using AutoMapper;
using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.Extensions;
using GRSMU.TimeTableBot.Common.Models;
using GRSMU.TimeTableBot.Core.DataLoaders;
using GRSMU.TimeTableBot.Core.Presenters;
using GRSMU.TimeTableBot.Data.TimeTables.Contracts;
using GRSMU.TimeTableBot.Data.TimeTables.Contracts.Filters;
using GRSMU.TimeTableBot.Domain.Dtos.TimeTable;
using GRSMU.TimeTableBot.Domain.RequestMessages.Timetables;
using Telegram.Bot;

namespace GRSMU.TimeTableBot.Application.Timetables.Handlers;

public class GetWeekTimeTableRequestHandler : GetTimeTableRequestHandlerBase<GetWeekTimeTableRequestMessage>
{
    public GetWeekTimeTableRequestHandler(ITelegramBotClient client, ITimeTableRepository timeTableRepository, TimeTablePresenter timeTablePresenter, IMapper mapper, ITimeTableLoader timeTableLoader) : base(client, timeTableRepository, timeTablePresenter, mapper, timeTableLoader)
    {
    }

    protected override TimeTableFilter CreateFilter(UserContext context)
    {
        var today = DateTime.Today;

        if (today.DayOfWeek is DayOfWeek.Saturday)
        {
            today = today.AddDays(2);
        }
        else if (today.DayOfWeek is DayOfWeek.Sunday)
        {
            today = today.AddDays(1);
        }

        var filter = new TimeTableFilter
        {
            GroupId = context.GroupId,
            Week = today.StartOfWeek()
        };

        return filter;
    }

    protected override async Task<List<TimeTableDto>> GetFromLoader(UserContext user, TimeTableFilter filter)
    {
        var startOfWeek = filter.Week ?? DateTime.Today.StartOfWeek();
        var endOfWeek = startOfWeek.EndOfWeek();
        
        var grabbedTimeTables = await TimeTableLoader.GrabTimeTableModels(new TimetableQuery
        {
            CourseId = user.CourseId,
            FacultyId = user.FacultyId,
            GroupId = user.GroupId,
            Week = startOfWeek.ToString("dd.MM.yyyy 0:00:00")
        });

        if (!grabbedTimeTables.Any())
        {
            return new List<TimeTableDto>();
        }
        
        grabbedTimeTables = grabbedTimeTables.Where(x => x.Date >= startOfWeek && x.Date <= endOfWeek).ToList();

        var dtos = Mapper.Map<List<TimeTableDto>>(grabbedTimeTables);

        return dtos;
    }
}