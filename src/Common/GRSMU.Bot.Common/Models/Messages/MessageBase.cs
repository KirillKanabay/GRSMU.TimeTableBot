using GRSMU.Bot.Common.Models.Responses;
using MediatR;

namespace GRSMU.Bot.Common.Models.Messages;

public abstract class MessageBase<TResponse> : IRequest<TResponse> where TResponse : ResponseBase
{
}