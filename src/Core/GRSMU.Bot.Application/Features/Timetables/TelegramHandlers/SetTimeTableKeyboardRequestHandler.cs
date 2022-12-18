using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Handlers;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Core.Immutable;
using GRSMU.Bot.Domain.Timetables.TelegramRequests;
using Telegram.Bot;

namespace GRSMU.Bot.Application.Features.Timetables.TelegramHandlers;

public class SetTimeTableKeyboardRequestHandler : SimpleTelegramRequestHandlerBase<SetTimeTableKeyboardRequestMessage>
{
    public SetTimeTableKeyboardRequestHandler(ITelegramBotClient client, ITelegramRequestContext context) : base(client, context)
    {
    }

    protected override async Task ExecuteAsync(SetTimeTableKeyboardRequestMessage request, CancellationToken cancellationToken)
    {
        await Client.SendTextMessageWithMarkup
        (
            Context.User,
            "Укажите период за который нужно показать расписание.",
            Markups.TimeTableMarkup
        );
    }
}