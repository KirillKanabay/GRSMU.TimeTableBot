namespace GRSMU.TimeTableBot.Common.Contexts
{
    public class UserContext : IUserContext
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

        public bool RegistrationCompleted => !string.IsNullOrWhiteSpace(GroupId) &&
                                             !string.IsNullOrWhiteSpace(CourseId) &&
                                             !string.IsNullOrWhiteSpace(FacultyId);
    }
}
