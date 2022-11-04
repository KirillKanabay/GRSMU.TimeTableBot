using GRSMU.TimeTableBot.Common.RequestMessages;
using Telegram.Bot.Types;

namespace GRSMU.TimeTableBot.Common.Broker.Messages.Factory
{
    public interface IRequestFactory
    {
        Task<TelegramRequestMessageBase> CreateRequestMessage(Update update);
    }
}
