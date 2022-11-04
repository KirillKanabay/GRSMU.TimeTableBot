using GRSMU.TimeTableBot.Common.Responses;

namespace GRSMU.TimeTableBot.Common.RequestMessages;

public class RequestMessageBase<TResponse> : IRequestMessage<TResponse>
    where TResponse : ResponseBase
{
}