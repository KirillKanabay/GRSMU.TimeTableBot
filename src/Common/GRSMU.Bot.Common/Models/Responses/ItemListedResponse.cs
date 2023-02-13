using GRSMU.Bot.Common.Models.Responses;

namespace GRSMU.Bot.Common.Models.Responses;

public abstract class ItemListedResponse<TData> : ResponseBase
{
    public List<TData> Items { get; set; }
}