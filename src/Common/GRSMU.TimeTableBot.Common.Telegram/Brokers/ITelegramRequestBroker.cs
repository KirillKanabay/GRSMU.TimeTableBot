using Telegram.Bot;
using Telegram.Bot.Types;

namespace GRSMU.TimeTableBot.Common.Telegram.Brokers
{
    public interface ITelegramRequestBroker
    {
        Task HandleUpdateAsync(Update update);

        Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken);
    }
}
