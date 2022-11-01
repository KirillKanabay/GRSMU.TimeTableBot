using GRSMU.TimeTableBot.Common.Broker.Handlers;
using GRSMU.TimeTableBot.Common.Broker.Responses;
using GRSMU.TimeTableBot.Common.Extensions;
using GRSMU.TimeTableBot.Common.Responses;
using GRSMU.TimeTableBot.Core.Immutable;
using GRSMU.TimeTableBot.Domain.RequestMessages.Timetables;
using Telegram.Bot;

namespace GRSMU.TimeTableBot.Application.Timetables.Handlers;

public class SetTimeTableKeyboardRequestHandler : RequestHandlerBase<SetTimeTableKeyboardRequestMessage>
{
    public SetTimeTableKeyboardRequestHandler(ITelegramBotClient client) : base(client)
    {
    }

    protected override async Task<EmptyResponse> ExecuteAsync(SetTimeTableKeyboardRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new EmptyResponse(request.UserContext, ResponseStatus.Finished);

        await Client.SendTextMessageWithMarkup(request.UserContext, "Укажите период за который нужно показать расписание.", Markups.TimeTableMarkup);

        return response;
    }
}