using GRSMU.Bot.Common.Telegram.Models.Messages;

namespace GRSMU.Bot.Domain.Users.TelegramRequests.Settings;

public abstract class SettingsRequestMessageBase : TelegramCommandMessageBase
{
    public string Value { get; set; }

    public bool BackExecuted { get; set; }
}