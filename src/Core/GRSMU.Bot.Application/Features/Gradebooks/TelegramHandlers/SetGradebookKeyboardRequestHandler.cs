using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Contracts;
using GRSMU.Bot.Common.Telegram.Brokers.Handlers;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Common.Telegram.Immutable;
using GRSMU.Bot.Domain.Gradebooks.TelegramRequests;
using Telegram.Bot;

namespace GRSMU.Bot.Application.Features.Gradebooks.TelegramHandlers;

public class SetGradebookKeyboardRequestHandler : SimpleTelegramRequestHandlerBase<SetGradebookKeyboardTelegramRequestMessage>
{
    private readonly ITelegramRequestBroker _requestBroker;

    public SetGradebookKeyboardRequestHandler(ITelegramBotClient client, ITelegramRequestContext context, ITelegramRequestBroker requestBroker) : base(client, context)
    {
        _requestBroker = requestBroker ?? throw new ArgumentNullException(nameof(requestBroker));
    }

    protected override Task ExecuteAsync(SetGradebookKeyboardTelegramRequestMessage request, CancellationToken cancellationToken)
    {
        return Context.User.IsStudentCardRegistered() ?
             Client.SendTextMessageWithMarkup(Context.User, "Журнал отметок", Markups.GradebookMarkup)
             : _requestBroker.Publish(new RunGradebookSettingsTelegramRequestMessage());
    }
}