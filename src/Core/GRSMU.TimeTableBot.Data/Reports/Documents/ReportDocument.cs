using GRSMU.TimeTable.Common.Data.Documents;
using GRSMU.TimeTableBot.Common.Data.Documents;

namespace GRSMU.TimeTableBot.Data.Reports.Documents;

public class ReportDocument : DocumentBase
{
    public string TelegramId { get; set; }

    public string Message { get; set; }
}