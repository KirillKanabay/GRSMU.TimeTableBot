using GRSMU.Bot.Common.Telegram.Models;

namespace GRSMU.Bot.Common.Telegram.Extensions;

public static class TelegramUserExtensions
{
    public static bool IsRegistered(this TelegramUser user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        return !string.IsNullOrWhiteSpace(user.GroupId) &&
               !string.IsNullOrWhiteSpace(user.CourseId) &&
               !string.IsNullOrWhiteSpace(user.FacultyId);
    }

    public static bool IsStudentCardRegistered(this TelegramUser user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        return !string.IsNullOrWhiteSpace(user.Login) &&
               !string.IsNullOrWhiteSpace(user.StudentCardId);
    }
}