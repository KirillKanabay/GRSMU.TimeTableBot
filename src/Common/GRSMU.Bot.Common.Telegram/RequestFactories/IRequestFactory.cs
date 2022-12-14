using GRSMU.Bot.Common.Models.RequestMessages;
using Telegram.Bot.Types;

namespace GRSMU.Bot.Common.Telegram.RequestFactories
{
    public interface IRequestFactory
    {
        Task<TelegramRequestMessageBase> CreateRequestMessage(Update update);
    }
}
