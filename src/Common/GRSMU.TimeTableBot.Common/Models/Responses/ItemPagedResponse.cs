namespace GRSMU.TimeTableBot.Common.Models.Responses;

public abstract class ItemPagedResponse<TData> : ItemListedResponse<TData>
{
    public PagingModel PagingModel { get; set; }
}