using GRSMU.Bot.Common.Models.Responses;

namespace GRSMU.Bot.Common.Models.Responses;

public class ItemPagedResponse<TData> : ItemListedResponse<TData>
{
    public PagingModel PagingModel { get; set; }
}