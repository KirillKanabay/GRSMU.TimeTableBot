using GRSMU.TimeTableBot.Common.Broker.Handlers;
using GRSMU.TimeTableBot.Common.Extensions;
using GRSMU.TimeTableBot.Common.Responses;
using GRSMU.TimeTableBot.Core.Immutable;
using GRSMU.TimeTableBot.Domain.RequestMessages.Common;
using Telegram.Bot;

namespace GRSMU.TimeTableBot.Application.Common.Handlers;

public class SetDefaultMenuRequestHandler : TelegramRequestHandlerBase<SetDefaultMenuRequestMessage>
{
    public SetDefaultMenuRequestHandler(ITelegramBotClient client) : base(client)
    {
    }

    protected override async Task<EmptyResponse> ExecuteAsync(SetDefaultMenuRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new EmptyResponse(request.UserContext, ResponseStatus.Finished);

        await Client.SendTextMessageWithMarkup(request.UserContext, "Главное меню", Markups.DefaultMarkup);

        return response;
    }
}