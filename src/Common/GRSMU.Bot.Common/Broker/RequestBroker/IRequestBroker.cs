using GRSMU.Bot.Common.Models.Responses;
using GRSMU.Bot.Common.Models.RequestMessages;
using GRSMU.Bot.Common.Models.Responses;

namespace GRSMU.Bot.Common.Broker.RequestBroker
{
    public interface IRequestBroker
    {
        public Task<TResponse> Publish<TResponse>(IRequestMessage<TResponse> request) 
            where TResponse : ResponseBase;
    }
}
