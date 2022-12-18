using Telegram.Bot;
using Telegram.Bot.Types;

namespace GRSMU.Bot.Common.Telegram.Brokers.Contracts
{
    public interface ITelegramUpdateHandler
    {
        Task HandleUpdateAsync(Update update);

        Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken);
    }
}
