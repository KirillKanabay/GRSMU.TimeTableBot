using AutoMapper;
using GRSMU.Bot.Application.Features.Timetables.DataLoaders;
using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Handlers;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Common.Telegram.Immutable;
using GRSMU.Bot.Common.Telegram.Models;
using GRSMU.Bot.Core.Presenters;
using GRSMU.Bot.Data.TimeTables.Contracts;
using GRSMU.Bot.Data.TimeTables.Contracts.Filters;
using GRSMU.Bot.Domain.Timetables.Dtos;
using GRSMU.Bot.Domain.Timetables.TelegramRequests;
using Telegram.Bot;

namespace GRSMU.Bot.Application.Features.Timetables.TelegramHandlers;

public abstract class GetTimeTableRequestHandlerBase<TRequest> : SimpleTelegramRequestHandlerBase<TRequest>
    where TRequest : GetTimeTableRequestMessageBase
{
    private readonly ITimeTableRepository _timeTableRepository;
    private readonly TimeTablePresenter _timeTablePresenter;
    protected readonly IMapper Mapper;
    protected readonly ITimeTableLoader TimeTableLoader;

    protected GetTimeTableRequestHandlerBase(ITelegramBotClient client, ITimeTableRepository timeTableRepository, TimeTablePresenter timeTablePresenter, IMapper mapper, ITimeTableLoader timeTableLoader, ITelegramRequestContext context) : base(client, context)
    {
        _timeTableRepository = timeTableRepository ?? throw new ArgumentNullException(nameof(timeTableRepository));
        _timeTablePresenter = timeTablePresenter ?? throw new ArgumentNullException(nameof(timeTablePresenter));
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        TimeTableLoader = timeTableLoader ?? throw new ArgumentNullException(nameof(timeTableLoader));
    }

    protected override async Task ExecuteAsync(TRequest request, CancellationToken cancellationToken)
    {
        var user = Context.User;

        if (!user.IsRegistered())
        {
            await Client.SendTextMessage(user, "Чтобы получить расписание нужно настроить профиль!");
            
            return;
        }

        var filter = CreateFilter(user);

        var timetables = await _timeTableRepository.GetDocuments(filter);

        List<TimeTableDto> dtos;

        if (timetables.Any())
        {
            dtos = Mapper.Map<List<TimeTableDto>>(timetables);
        }
        else
        {
            dtos = await GetFromLoader(user, filter);
        }

        if (!dtos.Any())
        {
            await Client.SendTextMessage(user, "Извините, расписание с такими параметрами не найдено, попробуйте позже!");

            return;
        }

        var timeTableText = _timeTablePresenter.ToString(dtos);

        await Client.SendTextMessageWithMarkup(user, timeTableText, Markups.DefaultMarkup);
    }

    protected abstract TimeTableFilter CreateFilter(TelegramUser context);

    protected abstract Task<List<TimeTableDto>> GetFromLoader(TelegramUser user, TimeTableFilter filter);
}