using GRSMU.Bot.Common.Models.Responses;
using GRSMU.Bot.Common.Telegram.Enums;

namespace GRSMU.Bot.Common.Telegram.Models.Responses;

public sealed class TelegramResponse
{
    public TelegramResponseStatus Status { get; set; }

    public TelegramResponse(TelegramResponseStatus status)
    {
    }
}