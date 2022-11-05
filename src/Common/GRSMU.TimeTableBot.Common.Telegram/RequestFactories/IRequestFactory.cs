using GRSMU.TimeTableBot.Common.RequestMessages;
using Telegram.Bot.Types;

namespace GRSMU.TimeTableBot.Common.Telegram.RequestFactories
{
    public interface IRequestFactory
    {
        Task<TelegramRequestMessageBase> CreateRequestMessage(Update update);
    }
}
