using GRSMU.Bot.Common.Contexts;
using Telegram.Bot.Types;

namespace GRSMU.Bot.Common.Services
{
    public interface IUserService
    {
        Task<UserContext> CreateContextFromTelegramUpdateAsync(Update update);

        Task UpdateContext(UserContext context);

        Task UpdateLastMessageBotId(UserContext context, int messageId);

        Task DeleteLastMessageBotId(UserContext context);
    }
}
