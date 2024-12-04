using GRSMU.Bot.Common.Enums;

namespace GRSMU.Bot.Logic.Features.Gradebook.Dtos;

public record MarkDto
{
    public DateTime? ParsedDate { get; set; }

    public string Date { get; set; }

    public string Mark { get; set; }

    public MarkType Type { get; set; }

    public MarkActivityType ActivityType { get; set; }
}