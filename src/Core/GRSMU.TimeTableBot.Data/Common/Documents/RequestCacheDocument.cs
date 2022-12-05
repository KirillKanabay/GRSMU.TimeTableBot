using GRSMU.TimeTable.Common.Data.Documents;
using GRSMU.TimeTableBot.Common.Data.Documents;

namespace GRSMU.TimeTableBot.Data.Common.Documents;

public class RequestCacheDocument : DocumentBase
{
    public string TelegramId { get; set; }

    public string Command { get; set; }
}