using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Handlers;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Core.Immutable;
using GRSMU.Bot.Domain.Common.TelegramRequests;
using Telegram.Bot;

namespace GRSMU.Bot.Application.Features.Common.TelegramHandlers;

public class SetDefaultMenuRequestHandler : SimpleTelegramRequestHandlerBase<SetDefaultMenuRequestMessage>
{
    public SetDefaultMenuRequestHandler(ITelegramBotClient client, ITelegramRequestContext context) : base(client, context)
    {
    }

    protected override Task ExecuteAsync(SetDefaultMenuRequestMessage request, CancellationToken cancellationToken)
    {
        return Client.SendTextMessageWithMarkup(Context.User, "Главное меню", Markups.DefaultMarkup);
    }
}