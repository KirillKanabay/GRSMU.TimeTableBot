using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Domain.Users.TelegramRequests.Settings;

public class FacultySettingsRequestMessage : SettingsRequestMessageBase
{
    public FacultySettingsRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}