using GRSMU.Bot.Common.Contexts;

namespace GRSMU.Bot.Domain.Users.TelegramRequests.Settings;

public class GroupSettingsRequestMessage : SettingsRequestMessageBase
{
    public GroupSettingsRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}