using Telegram.Bot.Types;
using TelegramRequestMessageBase = GRSMU.Bot.Common.Telegram.Models.Messages.TelegramRequestMessageBase;

namespace GRSMU.Bot.Common.Telegram.RequestFactories
{
    public interface IRequestFactory
    {
        Task<TelegramRequestMessageBase> CreateRequestMessage(Update update);
    }
}
