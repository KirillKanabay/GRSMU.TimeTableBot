using GRSMU.Bot.Common.Enums;

namespace GRSMU.Bot.Web.Api.Models.Gradebook;

public class MarkModel(
    DateTime? ParsedDate,
    string Date,
    string Mark,
    MarkType Type,
    MarkActivityType ActivityType);