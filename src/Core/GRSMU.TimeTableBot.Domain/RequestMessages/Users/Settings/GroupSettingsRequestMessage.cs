using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Domain.RequestMessages.Users.Settings;

public class GroupSettingsRequestMessage : SettingsRequestMessageBase
{
    public GroupSettingsRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}