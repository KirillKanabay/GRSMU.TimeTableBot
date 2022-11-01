using GRSMU.TimeTableBot.Common.Messages;
using GRSMU.TimeTableBot.Common.Responses;
using MediatR;

namespace GRSMU.TimeTableBot.Common.Broker.RequestBroker
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
