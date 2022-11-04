using GRSMU.TimeTableBot.Common.Models.Responses;
using MediatR;

namespace GRSMU.TimeTableBot.Common.Models.RequestMessages
{
    public interface IRequestMessage<out TResponse> : IRequest<TResponse> where TResponse : ResponseBase
    {
    }
}
