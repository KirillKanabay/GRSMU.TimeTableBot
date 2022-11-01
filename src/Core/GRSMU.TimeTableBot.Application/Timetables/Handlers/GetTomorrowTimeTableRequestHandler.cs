using AutoMapper;
using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.Extensions;
using GRSMU.TimeTableBot.Common.Models;
using GRSMU.TimeTableBot.Core.DataLoaders;
using GRSMU.TimeTableBot.Core.Presenters;
using GRSMU.TimeTableBot.Data.Repositories.TimeTables;
using GRSMU.TimeTableBot.Data.Repositories.TimeTables.Filters;
using GRSMU.TimeTableBot.Domain.Dtos;
using GRSMU.TimeTableBot.Domain.RequestMessages.Timetables;
using Telegram.Bot;

namespace GRSMU.TimeTableBot.Application.Timetables.Handlers;

public class GetTomorrowTimeTableRequestHandler : GetTimeTableRequestHandlerBase<GetTomorrowTimeTableRequestMessage>
{
    public GetTomorrowTimeTableRequestHandler(ITelegramBotClient client, ITimeTableRepository timeTableRepository, TimeTablePresenter timeTablePresenter, IMapper mapper, ITimeTableLoader timeTableLoader) : base(client, timeTableRepository, timeTablePresenter, mapper, timeTableLoader)
    {
    }

    protected override TimeTableFilter CreateFilter(UserContext context)
    {
        var day = DateTime.Today.AddDays(1);

        if (day.DayOfWeek is DayOfWeek.Saturday)
        {
            day = day.AddDays(2);
        }
        else if (day.DayOfWeek is DayOfWeek.Sunday)
        {
            day = day.AddDays(1);
        }

        var filter = new TimeTableFilter
        {
            GroupId = context.GroupId,
            Date = day
        };

        return filter;
    }

    protected override async Task<List<TimeTableDto>> GetFromLoader(UserContext user, TimeTableFilter filter)
    {
        var grabbedTimeTables = await TimeTableLoader.GrabTimeTableModels(new TimetableQuery
        {
            CourseId = user.CourseId,
            FacultyId = user.FacultyId,
            GroupId = user.GroupId,
            Week = filter.Date?.StartOfWeek().ToString("dd.MM.yyyy 0:00:00")
                   ?? DateTime.Today.StartOfWeek().ToString("dd.MM.yyyy 0:00:00")
        });

        if (!grabbedTimeTables.Any())
        {
            return new List<TimeTableDto>();
        }

        grabbedTimeTables = grabbedTimeTables.Where(x => x.Date.Equals(filter.Date)).ToList();

        var dtos = Mapper.Map<List<TimeTableDto>>(grabbedTimeTables);

        return dtos;
    }
}