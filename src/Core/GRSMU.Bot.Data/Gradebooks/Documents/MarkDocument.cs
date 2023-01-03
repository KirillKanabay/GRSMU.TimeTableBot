using GRSMU.Bot.Domain.Gradebooks.Enums;

namespace GRSMU.Bot.Data.Gradebooks.Documents;

public class MarkDocument
{
    public string Date { get; set; }

    public string Mark { get; set; }

    public MarkType Type { get; set; }
}