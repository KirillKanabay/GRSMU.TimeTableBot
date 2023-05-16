using GRSMU.Bot.Common.Data.Mongo.Documents;

namespace GRSMU.Bot.Data.Common.Documents;

public class RequestCacheDocument : DocumentBase
{
    public string TelegramId { get; set; }

    public string Command { get; set; }
}