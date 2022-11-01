using GRSMU.TimeTableBot.Common.Broker.Responses;
using GRSMU.TimeTableBot.Common.Responses;
using MediatR;

namespace GRSMU.TimeTableBot.Common.Messages
{
    public interface IRequestMessage<out TResponse> : IRequest<TResponse> where TResponse : ResponseBase
    {
    }
}
