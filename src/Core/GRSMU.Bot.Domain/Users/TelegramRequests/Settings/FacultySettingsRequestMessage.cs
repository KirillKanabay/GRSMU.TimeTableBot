using GRSMU.Bot.Common.Contexts;

namespace GRSMU.Bot.Domain.Users.TelegramRequests.Settings;

public class FacultySettingsRequestMessage : SettingsRequestMessageBase
{
    public FacultySettingsRequestMessage(UserContext userContext) : base(userContext)
    {
    }
}