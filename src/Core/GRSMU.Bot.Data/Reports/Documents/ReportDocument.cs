using GRSMU.Bot.Common.Data.Documents;

namespace GRSMU.Bot.Data.Reports.Documents;

public class ReportDocument : DocumentBase
{
    public string TelegramId { get; set; }

    public string Message { get; set; }
}