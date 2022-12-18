using GRSMU.Bot.Common.Telegram.Enums;

namespace GRSMU.Bot.Common.Telegram.Models.Responses;

public sealed class TelegramResponse
{
    public TelegramResponseStatus Status { get; private set; }

    public TelegramResponse(TelegramResponseStatus status)
    {
        Status = status;
    }
}