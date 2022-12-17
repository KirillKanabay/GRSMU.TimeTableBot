using GRSMU.Bot.Common.Telegram.Models.Messages;
using Telegram.Bot.Types;

namespace GRSMU.Bot.Common.Telegram.RequestFactories
{
    public interface IRequestFactory
    {
        Task<TelegramCommandMessageBase> CreateRequestMessage(Update update);
    }
}
