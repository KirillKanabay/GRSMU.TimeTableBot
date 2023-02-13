using GRSMU.Bot.Domain.Gradebooks.Enums;

namespace GRSMU.Bot.Domain.Gradebooks.Dtos;

public class MarkDto
{
    public string Date { get; set; }

    public string Mark { get; set; }

    public MarkType Type { get; set; }
}