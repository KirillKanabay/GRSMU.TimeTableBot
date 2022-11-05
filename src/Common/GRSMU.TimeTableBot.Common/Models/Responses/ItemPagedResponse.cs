namespace GRSMU.TimeTableBot.Common.Models.Responses;

public class ItemPagedResponse<TData> : ItemListedResponse<TData>
{
    public PagingModel PagingModel { get; set; }
}