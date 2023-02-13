using GRSMU.Bot.Common.Models.Responses;

namespace GRSMU.Bot.Common.Models.Messages;

public abstract class FilterPagedRequestMessage<TFilter, TResponse> : CommandMessageBase<TResponse>
    where TResponse : ResponseBase
{
    public TFilter Filter { get; set; }

    public PagingModel Paging { get; set; }
}