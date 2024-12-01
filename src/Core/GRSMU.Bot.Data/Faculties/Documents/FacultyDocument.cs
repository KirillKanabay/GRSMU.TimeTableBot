using GRSMU.Bot.Common.Data.Documents;

namespace GRSMU.Bot.Data.Faculties.Documents;

public class FacultyDocument : DocumentBase
{
    public string FacultyId { get; set; }

    public string FacultyName { get; set; }

    public string CourseId { get; set; }

    public string CourseName { get; set; }

    public List<GroupDocument> Groups { get; set; }
}