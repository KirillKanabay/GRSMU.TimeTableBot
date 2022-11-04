using GRSMU.TimeTableBot.Common.Responses;
using MediatR;

namespace GRSMU.TimeTableBot.Common.RequestMessages
{
    public interface IRequestMessage<out TResponse> : IRequest<TResponse> where TResponse : ResponseBase
    {
    }
}
