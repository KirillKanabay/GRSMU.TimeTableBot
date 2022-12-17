using GRSMU.Bot.Common.Telegram.Models;

namespace GRSMU.Bot.Common.Telegram.Extensions;

public abstract class TelegramUserExtensions
{
    public static bool IsRegistered(TelegramUser user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        return !string.IsNullOrWhiteSpace(user.GroupId) &&
               !string.IsNullOrWhiteSpace(user.CourseId) &&
               !string.IsNullOrWhiteSpace(user.FacultyId);
    }
}