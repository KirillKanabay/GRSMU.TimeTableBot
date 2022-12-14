using GRSMU.Bot.Common.Models.RequestMessages;
using GRSMU.Bot.Common.Contexts;

namespace GRSMU.Bot.Domain.Users.TelegramRequests.Settings;

public abstract class SettingsRequestMessageBase : TelegramRequestMessageBase
{
    protected SettingsRequestMessageBase(UserContext userContext) : base(userContext)
    {
    }

    public string Value { get; set; }

    public bool BackExecuted { get; set; }
}