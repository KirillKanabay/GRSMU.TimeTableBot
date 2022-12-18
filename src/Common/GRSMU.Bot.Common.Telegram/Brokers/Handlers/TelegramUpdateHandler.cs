using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Contracts;
using GRSMU.Bot.Common.Telegram.Brokers.RequestCache;
using GRSMU.Bot.Common.Telegram.Enums;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Common.Telegram.RequestFactories;
using GRSMU.Bot.Common.Telegram.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GRSMU.Bot.Common.Telegram.Brokers.Handlers;

public class TelegramUpdateHandler : ITelegramUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly ITelegramUserService _userService;
    private readonly ILogger<TelegramUpdateHandler> _logger;
    private readonly IServiceProvider _serviceProvider;

    public TelegramUpdateHandler(ITelegramBotClient botClient, ITelegramUserService userService, ILogger<TelegramUpdateHandler> logger, ITelegramRequestContext context, IServiceProvider serviceProvider)
    {
        _botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }
    
    public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        => HandleUpdateAsync(update);

    public async Task HandleUpdateAsync(Update update)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetService<ITelegramRequestContext>();

            if (ctx == null)
            {
                throw new NullReferenceException(nameof(ctx));
            }

            await FillRequestContext(ctx, update);

            var requestFactory = scope.ServiceProvider.GetService<IRequestFactory>();

            var request = await requestFactory.CreateRequestMessage(update);

            var telegramUser = ctx.User;

            if (request == null)
            {
                await _botClient.SendTextMessage(telegramUser, "Такой команды не существует!");

                return;
            }

            try
            {
                var requestBroker = scope.ServiceProvider.GetService<ITelegramRequestBroker>();
                var requestCache = scope.ServiceProvider.GetService<IRequestCache>();

                var response = await requestBroker.Publish(request);

                if (response.Status is TelegramResponseStatus.WaitingNextResponse)
                {
                    await requestCache.Push(telegramUser.TelegramId, update.ExtractCommand());
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Error while processing telegram request: {e}", e);

                await _botClient.SendTextMessage(telegramUser, "Упс! Во время обработки вашего запроса, бот неожиданно сломался(");
            }
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogWarning($"Error while processing telegram request:{exception}", exception);

        return Task.CompletedTask;
    }

    private async Task FillRequestContext(ITelegramRequestContext ctx, Update update)
    {
        ctx.Update = update;
        ctx.User = await _userService.CreateUserFromTelegramUpdateAsync(update);
    }
}