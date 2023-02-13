using GRSMU.Bot.Common.Models.Responses;
using MediatR;
using GRSMU.Bot.Common.Broker.Contracts;
using GRSMU.Bot.Common.Models.Messages;

namespace GRSMU.Bot.Common.Broker
{
    public class MediatorRequestBroker : IRequestBroker
    {
        private readonly IMediator _mediator;

        public MediatorRequestBroker(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<TResponse> Publish<TResponse>(IRequestMessage<TResponse> request) where TResponse : ResponseBase
        {
            var response = await _mediator.Send(request);

            return response;
        }

        public Task PublishEvent(EventMessageBase @event)
        {
            return _mediator.Publish(@event);
        }
    }
}
