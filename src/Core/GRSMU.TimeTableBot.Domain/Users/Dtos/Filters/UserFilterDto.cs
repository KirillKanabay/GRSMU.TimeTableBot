namespace GRSMU.TimeTableBot.Domain.Users.Dtos.Filters
{
    public class UserFilterDto
    {
        public List<string> GroupIds { get; set; }

        public List<string> CourseIds { get; set; }

        public List<string> FacultyIds { get; set; }
    }
}
