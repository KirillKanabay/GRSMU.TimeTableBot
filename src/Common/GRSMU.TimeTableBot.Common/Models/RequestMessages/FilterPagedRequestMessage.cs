using GRSMU.TimeTableBot.Common.Models.Responses;
using GRSMU.TimeTableBot.Common.RequestMessages;

namespace GRSMU.TimeTableBot.Common.Models.RequestMessages;

public abstract class FilterPagedRequestMessage<TFilter, TResponse> : RequestMessageBase<TResponse>
    where TResponse : ResponseBase
{
    public TFilter Filter { get; set; }

    public PagingModel Paging { get; set; }
}