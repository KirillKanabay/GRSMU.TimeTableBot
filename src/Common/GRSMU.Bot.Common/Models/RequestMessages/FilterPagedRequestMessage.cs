using GRSMU.Bot.Common.Models.Responses;
using GRSMU.Bot.Common.Models.Responses;

namespace GRSMU.Bot.Common.Models.RequestMessages;

public abstract class FilterPagedRequestMessage<TFilter, TResponse> : RequestMessageBase<TResponse>
    where TResponse : ResponseBase
{
    public TFilter Filter { get; set; }

    public PagingModel Paging { get; set; }
}