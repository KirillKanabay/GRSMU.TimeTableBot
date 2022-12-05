namespace GRSMU.TimeTableBot.Data.Users.Contracts.Filters
{
    public class UserFilter
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<string> GroupIds { get; set; }

        public List<string> CourseIds { get; set; }

        public List<string> FacultyIds { get; set; }
    }
}
