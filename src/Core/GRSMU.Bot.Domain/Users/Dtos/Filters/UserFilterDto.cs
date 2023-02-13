namespace GRSMU.Bot.Domain.Users.Dtos.Filters
{
    public class UserFilterDto
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<string> GroupIds { get; set; }

        public List<string> CourseIds { get; set; }

        public List<string> FacultyIds { get; set; }
    }
}
