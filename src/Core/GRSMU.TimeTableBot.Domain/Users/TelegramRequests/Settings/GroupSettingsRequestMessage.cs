using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Domain.Users.TelegramRequests.Settings;

public class GroupSettingsRequestMessage : SettingsRequestMessageBase
{
    public GroupSettingsRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}