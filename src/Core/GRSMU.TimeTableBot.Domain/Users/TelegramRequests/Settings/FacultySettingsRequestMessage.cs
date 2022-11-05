using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Domain.Users.TelegramRequests.Settings;

namespace GRSMU.TimeTableBot.Domain.Users.Requests.Settings;

public class FacultySettingsRequestMessage : SettingsRequestMessageBase
{
    public FacultySettingsRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}