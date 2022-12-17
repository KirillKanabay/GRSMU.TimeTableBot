using GRSMU.Bot.Common.Models.Responses;
using GRSMU.Bot.Common.Models.Responses;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Core.Immutable;
using GRSMU.Bot.Domain.Timetables.Requests;
using Telegram.Bot;
using GRSMU.Bot.Common.Telegram.Brokers.Handlers;
using GRSMU.Bot.Common.Telegram.Enums;
using GRSMU.Bot.Domain.Timetables.TelegramRequests;

namespace GRSMU.Bot.Application.Timetables.TelegramHandlers;

public class SetTimeTableKeyboardRequestHandler : TelegramRequestHandlerBase<SetTimeTableKeyboardRequestMessage>
{
    public SetTimeTableKeyboardRequestHandler(ITelegramBotClient client) : base(client)
    {
    }

    protected override async Task<TelegramResponse> ExecuteAsync(SetTimeTableKeyboardRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new TelegramResponse(request.UserContext, TelegramResponseStatus.Finished);

        await Client.SendTextMessageWithMarkup(request.UserContext, "Укажите период за который нужно показать расписание.", Markups.TimeTableMarkup);

        return response;
    }
}