using GRSMU.Bot.Common.Telegram.Models;
using Telegram.Bot.Types;

namespace GRSMU.Bot.Common.Telegram.Services
{
    public interface ITelegramUserService
    {
        Task<TelegramUser> CreateUserFromTelegramUpdateAsync(Update update);

        Task UpdateUserAsync(TelegramUser user);

        Task UpdateLastMessageBotIdAsync(TelegramUser user, int messageId);

        Task DeleteLastMessageBotIdAsync(TelegramUser user);
    }
}
