using GRSMU.Bot.Common.Contexts;

namespace GRSMU.Bot.Domain.Users.TelegramRequests.Settings;

public class CourseSettingsRequestMessage : SettingsRequestMessageBase
{
    public CourseSettingsRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}