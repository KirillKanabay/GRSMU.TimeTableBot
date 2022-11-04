using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.RequestMessages;

namespace GRSMU.TimeTableBot.Domain.RequestMessages.Users.Settings;

public class SettingsRequestMessageBase : TelegramRequestMessageBase
{
    public SettingsRequestMessageBase(UserContext userContext) : base(userContext)
    {
    }

    public string Value { get; set; }

    public bool BackExecuted { get; set; }
}