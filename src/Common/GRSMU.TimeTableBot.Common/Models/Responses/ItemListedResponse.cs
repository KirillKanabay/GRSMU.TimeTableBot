namespace GRSMU.TimeTableBot.Common.Models.Responses;

public abstract class ItemListedResponse<TData> : ResponseBase
{
    public List<TData> Items { get; set; }
}