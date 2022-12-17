using GRSMU.Bot.Common.Models.Responses;

namespace GRSMU.Bot.Common.Models.Messages;

public abstract class QueryMessageBase<TResponse> : MessageBase<TResponse>
    where TResponse : ResponseBase
{
    
}