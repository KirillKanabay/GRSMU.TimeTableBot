using GRSMU.TimeTableBot.Common.Broker.Messages.Factory;
using GRSMU.TimeTableBot.Common.Broker.RequestBroker;
using GRSMU.TimeTableBot.Common.Broker.RequestCache;
using GRSMU.TimeTableBot.Common.Broker.Responses;
using GRSMU.TimeTableBot.Common.Extensions;
using GRSMU.TimeTableBot.Common.Services;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GRSMU.TimeTableBot.Common.Broker.TelegramBroker;

public class TelegramRequestBroker : ITelegramRequestBroker
{
    private readonly IRequestBroker _requestBroker;
    private readonly IRequestCache _requestCache;
    private readonly IRequestFactory _requestFactory;
    private readonly ITelegramBotClient _botClient;
    private readonly IUserService _userService;
    private readonly ILogger<TelegramRequestBroker> _logger;

    public TelegramRequestBroker(IRequestBroker requestBroker, IRequestCache requestCache, IRequestFactory requestFactory, ITelegramBotClient botClient, IUserService userService, ILogger<TelegramRequestBroker> logger)
    {
        _requestBroker = requestBroker ?? throw new ArgumentNullException(nameof(requestBroker));
        _requestCache = requestCache ?? throw new ArgumentNullException(nameof(requestCache));
        _requestFactory = requestFactory ?? throw new ArgumentNullException(nameof(requestFactory));
        _botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) 
        => HandleUpdateAsync(update);

    public async Task HandleUpdateAsync(Update update)
    {
        var request = await _requestFactory.CreateRequestMessage(update);

        var userContext = await _userService.CreateContextFromTelegramUpdateAsync(update);

        if (request == null)
        {
            await _botClient.SendTextMessage(userContext, "Такой команды не существует!");

            return;
        }

        try
        {
            var response = await _requestBroker.Publish(request);

            if (response.Status is ResponseStatus.WaitingNextResponse)
            {
                await _requestCache.Push(request.UserContext.TelegramId, update.ExtractCommand());
            }
        }
        catch (Exception e)
        {
            _logger.LogWarning($"Error while processing telegram request: {e}", e);

            await _botClient.SendTextMessage(userContext, "Упс! Во время обработки вашего запроса, бот неожиданно сломался(");
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogWarning($"Error while processing telegram request:{exception}", exception);

        return Task.CompletedTask;
    }
}