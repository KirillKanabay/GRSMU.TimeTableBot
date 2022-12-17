using GRSMU.Bot.Common.Models.Responses;
using GRSMU.Bot.Common.Extensions;
using GRSMU.Bot.Common.Models.Responses;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Common.Telegram.Models.Responses;
using GRSMU.Bot.Core.Immutable;
using Telegram.Bot;
using GRSMU.Bot.Common.Telegram.Brokers.Handlers;
using GRSMU.Bot.Common.Telegram.Enums;
using GRSMU.Bot.Domain.Common.TelegramRequests;

namespace GRSMU.Bot.Application.Features.Common.Handlers;

public class SetDefaultMenuRequestHandler : TelegramRequestHandlerBase<SetDefaultMenuRequestMessage>
{
    public SetDefaultMenuRequestHandler(ITelegramBotClient client) : base(client)
    {
    }

    protected override async Task<TelegramResponse> ExecuteAsync(SetDefaultMenuRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new TelegramResponse(TelegramResponseStatus.Finished);

        await Client.SendTextMessageWithMarkup(request.UserContext, "Главное меню", Markups.DefaultMarkup);

        return response;
    }
}