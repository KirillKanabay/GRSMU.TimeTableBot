using GRSMU.Bot.Common.Models.Responses;
using GRSMU.Bot.Common.Models.Responses;

namespace GRSMU.Bot.Common.Models.RequestMessages;

public abstract class RequestMessageBase<TResponse> : IRequestMessage<TResponse>
    where TResponse : ResponseBase
{
}

public abstract class RequestMessageBase : RequestMessageBase<EmptyResponse>
{
}