using GRSMU.TimeTableBot.Common.Messages;
using GRSMU.TimeTableBot.Common.RequestMessages;
using Telegram.Bot.Types;

namespace GRSMU.TimeTableBot.Common.Broker.Messages.Factory
{
    public interface IRequestFactory
    {
        Task<RequestMessageBase> CreateRequestMessage(Update update);
    }
}
