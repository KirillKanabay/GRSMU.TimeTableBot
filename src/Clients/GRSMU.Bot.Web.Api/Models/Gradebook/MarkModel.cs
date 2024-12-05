using GRSMU.Bot.Common.Enums;

namespace GRSMU.Bot.Web.Api.Models.Gradebook;

public record MarkModel
{
    public DateTime? ParsedDate { get; set; }

    public string Date { get; set; }

    public string Mark { get; set; }

    public MarkType Type { get; set; }

    public MarkActivityType ActivityType { get; set; }
}