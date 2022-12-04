namespace GRSMU.TimeTableBot.Common.Models;

public class PagingModel
{
    public int Page { get; set; }

    public int PageSize { get; set; }

    public string SortBy { get; set; }

    public int TotalCount { get; set; }

    public int PagesCount { get; set; }
}