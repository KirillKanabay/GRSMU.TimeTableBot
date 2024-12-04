using GRSMU.Bot.Common.Enums;
using MarkType = GRSMU.Bot.Domain.Gradebooks.Enums.MarkType;

namespace GRSMU.Bot.Data.Gradebooks.Documents;

public class MarkDocument
{
    public DateTime? Date { get; set; }

    public string OriginalDate { get; set; }

    public string Mark { get; set; }

    public MarkType Type { get; set; }

    public MarkActivityType ActivityType { get; set; }
}