namespace GRSMU.TimeTableBot.Domain.Users.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }

        public string TelegramId { get; set; }

        public double ChatId { get; set; }

        public bool IsAdmin { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string GroupId { get; set; }

        public string CourseId { get; set; }

        public string FacultyId { get; set; }

        public string GroupName { get; set; }

        public string FacultyName { get; set; }

        public string CourseName { get; set; }
    }
}
