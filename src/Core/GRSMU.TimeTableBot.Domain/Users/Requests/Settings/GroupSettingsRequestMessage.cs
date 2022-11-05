using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Domain.Users.Requests.Settings;

public class GroupSettingsRequestMessage : SettingsRequestMessageBase
{
    public GroupSettingsRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}