using GRSMU.Bot.Common.Models.Responses;

namespace GRSMU.Bot.Common.Models.Messages;

public abstract class CommandMessageBase<TResponse> : MessageBase<TResponse>
    where TResponse : ResponseBase
{
}

public abstract class CommandMessageBase : CommandMessageBase<EmptyResponse>
{
}