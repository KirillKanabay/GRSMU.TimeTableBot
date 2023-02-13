using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Contracts;
using GRSMU.Bot.Common.Telegram.Brokers.Handlers;
using GRSMU.Bot.Domain.Gradebooks.TelegramRequests;
using GRSMU.Bot.Domain.Gradebooks.TelegramRequests.Settings;
using Telegram.Bot;

namespace GRSMU.Bot.Application.Features.Gradebooks.TelegramHandlers.GradebookSettings;

public class RunGradebookSettingsTelegramRequestHandler : SimpleTelegramRequestHandlerBase<RunGradebookSettingsTelegramRequestMessage>
{
    private readonly ITelegramRequestBroker _requestBroker;

    public RunGradebookSettingsTelegramRequestHandler(ITelegramBotClient client, ITelegramRequestContext context, ITelegramRequestBroker requestBroker) : base(client, context)
    {
        _requestBroker = requestBroker ?? throw new ArgumentNullException(nameof(requestBroker));
    }

    protected override Task ExecuteAsync(RunGradebookSettingsTelegramRequestMessage request, CancellationToken cancellationToken)
    {
        return _requestBroker.Publish(new SetGradebookLoginTelegramRequestMessage());
    }
}