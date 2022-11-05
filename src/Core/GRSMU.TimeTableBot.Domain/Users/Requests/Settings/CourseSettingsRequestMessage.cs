using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Domain.Users.Requests.Settings;

public class CourseSettingsRequestMessage : SettingsRequestMessageBase
{
    public CourseSettingsRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}