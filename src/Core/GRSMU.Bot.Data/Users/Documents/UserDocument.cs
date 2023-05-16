using GRSMU.Bot.Common.Data.Mongo.Documents;

namespace GRSMU.Bot.Data.Users.Documents
{
    public class UserDocument : DocumentBase
    {
        public string TelegramId { get; set; }

        public int? LastBotMessageId { get; set; }

        public long ChatId { get; set; }

        public bool IsAdmin { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string GroupId { get; set; }

        public string CourseId { get; set; }

        public string FacultyId { get; set; }

        public string Login { get; set; }

        public string StudentCardId { get; set; }

        public string StudentFullName { get; set; }
    }
}
