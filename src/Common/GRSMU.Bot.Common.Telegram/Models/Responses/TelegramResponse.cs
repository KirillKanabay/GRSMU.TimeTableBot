using GRSMU.Bot.Common.Models.Responses;

namespace GRSMU.Bot.Common.Telegram.Models.Responses;

public sealed class TelegramResponse
{
    public TelegramResponseStatus Status { get; set; }

    public TelegramResponse(TelegramResponseStatus status)
    {
    }
}