using GRSMU.TimeTableBot.Common.Broker.Messages;
using GRSMU.TimeTableBot.Common.Models.RequestMessages;
using GRSMU.TimeTableBot.Common.Models.Responses;
using GRSMU.TimeTableBot.Common.RequestMessages;

namespace GRSMU.TimeTableBot.Common.Broker.RequestBroker
{
    public interface IRequestBroker
    {
        public Task<TResponse> Publish<TResponse>(IRequestMessage<TResponse> request) 
            where TResponse : ResponseBase;
    }
}
