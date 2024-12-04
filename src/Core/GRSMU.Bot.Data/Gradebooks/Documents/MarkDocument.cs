﻿using GRSMU.Bot.Common.Enums;

namespace GRSMU.Bot.Data.Gradebooks.Documents;

public class MarkDocument
{
    public DateTime? ParsedDate { get; set; }

    public string Date { get; set; }

    public string Mark { get; set; }

    public MarkType Type { get; set; }

    public MarkActivityType ActivityType { get; set; }
}