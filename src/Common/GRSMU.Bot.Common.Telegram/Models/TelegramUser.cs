namespace GRSMU.Bot.Common.Telegram.Models
{
    public class TelegramUser
    {
        public string MongoId { get; set; }

        public int? LastBotMessageId { get; set; }

        public bool IsAdmin { get; set; }

        public string TelegramId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public long ChatId { get; set; }

        public string GroupId { get; set; }

        public string CourseId { get; set; }

        public string FacultyId { get; set; }

        public string Login { get; set; }

        public string StudentCardId { get; set; }

        public string StudentFullName { get; set; }
    }
}
