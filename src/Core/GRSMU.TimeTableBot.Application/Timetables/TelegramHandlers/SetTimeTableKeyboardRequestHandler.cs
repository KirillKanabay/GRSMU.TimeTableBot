using GRSMU.TimeTableBot.Common.Models.Responses;
using GRSMU.TimeTableBot.Common.Telegram.Extensions;
using GRSMU.TimeTableBot.Common.Telegram.Handlers;
using GRSMU.TimeTableBot.Core.Immutable;
using GRSMU.TimeTableBot.Domain.Timetables.Requests;
using Telegram.Bot;

namespace GRSMU.TimeTableBot.Application.Timetables.TelegramHandlers;

public class SetTimeTableKeyboardRequestHandler : TelegramRequestHandlerBase<SetTimeTableKeyboardRequestMessage>
{
    public SetTimeTableKeyboardRequestHandler(ITelegramBotClient client) : base(client)
    {
    }

    protected override async Task<TelegramResponse> ExecuteAsync(SetTimeTableKeyboardRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new TelegramResponse(request.UserContext, ResponseStatus.Finished);

        await Client.SendTextMessageWithMarkup(request.UserContext, "Укажите период за который нужно показать расписание.", Markups.TimeTableMarkup);

        return response;
    }
}