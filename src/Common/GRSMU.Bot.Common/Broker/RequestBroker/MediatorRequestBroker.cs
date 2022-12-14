using GRSMU.Bot.Common.Models.Responses;
using GRSMU.Bot.Common.Models.RequestMessages;
using GRSMU.Bot.Common.Models.Responses;
using MediatR;

namespace GRSMU.Bot.Common.Broker.RequestBroker
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
    }
}
