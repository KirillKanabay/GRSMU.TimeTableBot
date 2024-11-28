using GRSMU.Bot.Common.Models.Messages;
using GRSMU.Bot.Common.Models.Responses;

namespace GRSMU.Bot.Common.Broker.Contracts
{
    public interface IRequestBroker
    {
        public Task<TResponse> Publish<TResponse>(IRequestMessage<TResponse> request)
            where TResponse : ResponseBase;

        Task PublishEvent(EventMessageBase @event);
    }
}
