using AutoMapper;
using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.Models.Responses;
using GRSMU.TimeTableBot.Common.Telegram.Extensions;
using GRSMU.TimeTableBot.Common.Telegram.Handlers;
using GRSMU.TimeTableBot.Core.DataLoaders;
using GRSMU.TimeTableBot.Core.Immutable;
using GRSMU.TimeTableBot.Core.Presenters;
using GRSMU.TimeTableBot.Data.TimeTables.Contracts;
using GRSMU.TimeTableBot.Data.TimeTables.Contracts.Filters;
using GRSMU.TimeTableBot.Domain.Timetables.Dtos;
using GRSMU.TimeTableBot.Domain.Timetables.Requests;
using Telegram.Bot;

namespace GRSMU.TimeTableBot.Application.Timetables.Handlers;

public abstract class GetTimeTableRequestHandlerBase<TRequest> : TelegramRequestHandlerBase<TRequest>
    where TRequest : GetTimeTableRequestMessageBase
{
    private readonly ITimeTableRepository _timeTableRepository;
    private readonly TimeTablePresenter _timeTablePresenter;
    protected readonly IMapper Mapper;
    protected readonly ITimeTableLoader TimeTableLoader;

    protected GetTimeTableRequestHandlerBase(ITelegramBotClient client, ITimeTableRepository timeTableRepository, TimeTablePresenter timeTablePresenter, IMapper mapper, ITimeTableLoader timeTableLoader) : base(client)
    {
        _timeTableRepository = timeTableRepository ?? throw new ArgumentNullException(nameof(timeTableRepository));
        _timeTablePresenter = timeTablePresenter ?? throw new ArgumentNullException(nameof(timeTablePresenter));
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        TimeTableLoader = timeTableLoader ?? throw new ArgumentNullException(nameof(timeTableLoader));
    }

    protected override async Task<EmptyResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken)
    {
        var response = new EmptyResponse(request.UserContext, ResponseStatus.Finished);
        
        var user = request.UserContext;

        if (!user.RegistrationCompleted)
        {
            await Client.SendTextMessage(user, "Чтобы получить расписание нужно настроить профиль!");
            
            return response;
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
            await Client.SendTextMessage(request.UserContext, "Извините, расписание с такими параметрами не найдено, попробуйте позже!");

            return response;
        }

        var timeTableText = _timeTablePresenter.ToString(dtos);

        await Client.SendTextMessageWithMarkup(request.UserContext, timeTableText, Markups.DefaultMarkup);

        return response;
    }

    protected abstract TimeTableFilter CreateFilter(UserContext context);

    protected abstract Task<List<TimeTableDto>> GetFromLoader(UserContext user, TimeTableFilter filter);
}