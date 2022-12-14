using GRSMU.Bot.Common.Data.Documents;

namespace GRSMU.Bot.Data.Common.Documents;

public class RequestCacheDocument : DocumentBase
{
    public string TelegramId { get; set; }

    public string Command { get; set; }
}