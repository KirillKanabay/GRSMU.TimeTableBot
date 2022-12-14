using GRSMU.TimeTableBot.Common.Models.Responses;

namespace GRSMU.TimeTableBot.Common.Models.RequestMessages;

public abstract class RequestMessageBase<TResponse> : IRequestMessage<TResponse>
    where TResponse : ResponseBase
{
}

public abstract class RequestMessageBase : RequestMessageBase<EmptyResponse>
{
}