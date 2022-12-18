using AutoMapper;
using GRSMU.Bot.Application.Features.Timetables.DataLoaders;
using GRSMU.Bot.Common.Extensions;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Models;
using GRSMU.Bot.Core.Presenters;
using GRSMU.Bot.Data.TimeTables.Contracts;
using GRSMU.Bot.Data.TimeTables.Contracts.Filters;
using GRSMU.Bot.Domain.Timetables.Dtos;
using GRSMU.Bot.Domain.Timetables.TelegramRequests;
using Telegram.Bot;

namespace GRSMU.Bot.Application.Features.Timetables.TelegramHandlers;

public class GetWeekTimeTableRequestHandler : GetTimeTableRequestHandlerBase<GetWeekTimeTableRequestMessage>
{
    public GetWeekTimeTableRequestHandler(
        ITelegramBotClient client, 
        ITimeTableRepository timeTableRepository, 
        TimeTablePresenter timeTablePresenter, 
        IMapper mapper, 
        ITimeTableLoader timeTableLoader,
        ITelegramRequestContext context) : base(client, timeTableRepository, timeTablePresenter, mapper, timeTableLoader, context)
    {
    }

    protected override TimeTableFilter CreateFilter(TelegramUser context)
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

    protected override async Task<List<TimeTableDto>> GetFromLoader(TelegramUser user, TimeTableFilter filter)
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