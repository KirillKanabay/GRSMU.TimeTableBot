using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Domain.RequestMessages.Users.Settings;

public class CourseSettingsRequestMessage : SettingsRequestMessageBase
{
    public CourseSettingsRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}