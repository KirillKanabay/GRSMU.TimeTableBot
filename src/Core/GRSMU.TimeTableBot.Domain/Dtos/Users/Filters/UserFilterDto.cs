namespace GRSMU.TimeTableBot.Domain.Dtos.Users.Filters
{
    public class UserFilterDto
    {
        public List<string> GroupIds { get; set; }

        public List<string> CourseIds { get; set; }

        public List<string> FacultyIds { get; set; }
    }
}
