using GRSMU.TimeTable.Common.Data.Documents;

namespace GRSMU.TimeTableBot.Data.Documents;

public class RequestCacheDocument : DocumentBase
{
    public string TelegramId { get; set; }

    public string Command { get; set; }
}