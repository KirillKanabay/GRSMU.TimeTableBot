using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Domain.Users.TelegramRequests.Settings;

namespace GRSMU.TimeTableBot.Domain.Users.Requests.Settings;

public class GroupSettingsRequestMessage : SettingsRequestMessageBase
{
    public GroupSettingsRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}