using AutoMapper;
using GRSMU.Bot.Common.Contexts;
using GRSMU.Bot.Common.Extensions;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Core.DataLoaders;
using GRSMU.Bot.Core.Presenters;
using GRSMU.Bot.Data.TimeTables.Contracts;
using GRSMU.Bot.Data.TimeTables.Contracts.Filters;
using GRSMU.Bot.Domain.Timetables.Dtos;
using GRSMU.Bot.Domain.Timetables.Requests;
using GRSMU.Bot.Domain.Timetables.TelegramRequests;
using Telegram.Bot;

namespace GRSMU.Bot.Application.Timetables.TelegramHandlers;

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