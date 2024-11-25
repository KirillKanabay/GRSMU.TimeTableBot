using GRSMU.Bot.Common.Data.Documents;

namespace GRSMU.Bot.Data.Users.Documents
{
    public class UserDocument : DocumentBase
    {
        public string TelegramId { get; set; }

        public long ChatId { get; set; }

        public bool IsAdmin { get; set; }

        public string TelegramFirstName { get; set; }

        public string TelegramLastName { get; set; }

        public string TelegramUsername { get; set; }

        public string GroupId { get; set; }

        public string CourseId { get; set; }

        public string FacultyId { get; set; }

        public string StudentCardId { get; set; }

        public string StudentCardPassword { get; set; }

        public string StudentFullName { get; set; }
    }
}
