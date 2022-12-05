using GRSMU.TimeTableBot.Common.Models.RequestMessages;
using GRSMU.TimeTableBot.Common.Models.Responses;

namespace GRSMU.TimeTableBot.Common.RequestMessages;

public class RequestMessageBase<TResponse> : IRequestMessage<TResponse>
    where TResponse : ResponseBase
{
}