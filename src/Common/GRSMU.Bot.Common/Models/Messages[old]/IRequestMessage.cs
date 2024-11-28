using GRSMU.Bot.Common.Models.Responses;
using MediatR;

namespace GRSMU.Bot.Common.Models.Messages
{
    public interface IRequestMessage<out TResponse> : IRequest<TResponse> where TResponse : ResponseBase
    {
    }
}
