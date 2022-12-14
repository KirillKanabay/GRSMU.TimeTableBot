using GRSMU.TimeTableBot.Common.Extensions;
using GRSMU.TimeTableBot.Common.Models.Responses;
using GRSMU.TimeTableBot.Common.Telegram.Extensions;
using GRSMU.TimeTableBot.Common.Telegram.Handlers;
using GRSMU.TimeTableBot.Core.Immutable;
using GRSMU.TimeTableBot.Domain.Common.Requests;
using Telegram.Bot;

namespace GRSMU.TimeTableBot.Application.Common.Handlers;

public class SetDefaultMenuRequestHandler : TelegramRequestHandlerBase<SetDefaultMenuRequestMessage>
{
    public SetDefaultMenuRequestHandler(ITelegramBotClient client) : base(client)
    {
    }

    protected override async Task<TelegramResponse> ExecuteAsync(SetDefaultMenuRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new TelegramResponse(request.UserContext, ResponseStatus.Finished);

        await Client.SendTextMessageWithMarkup(request.UserContext, "Главное меню", Markups.DefaultMarkup);

        return response;
    }
}