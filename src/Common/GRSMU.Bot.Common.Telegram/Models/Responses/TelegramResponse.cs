using GRSMU.Bot.Common.Telegram.Enums;

namespace GRSMU.Bot.Common.Telegram.Models.Responses;

public sealed class TelegramResponse
{
    public TelegramResponseStatus Status { get; private set; } = TelegramResponseStatus.Finished;
    
    public string Command { get; set; }

    public TelegramResponse()
    {
    }

    public TelegramResponse(TelegramResponseStatus status, string command)
    {
        Status = status;
        Command = command;
    }
}