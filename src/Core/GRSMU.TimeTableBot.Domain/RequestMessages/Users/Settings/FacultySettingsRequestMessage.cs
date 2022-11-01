using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Domain.RequestMessages.Users.Settings;

public class FacultySettingsRequestMessage : SettingsRequestMessageBase
{
    public FacultySettingsRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}