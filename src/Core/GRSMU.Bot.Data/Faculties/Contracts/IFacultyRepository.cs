using GRSMU.Bot.Data.Common.Documents;
using GRSMU.Bot.Data.Faculties.Documents;

namespace GRSMU.Bot.Data.Faculties.Contracts;

public interface IFacultyRepository
{
    Task<List<FacultyDocument>> SearchByFacultyIdAsync(string facultyId);

    Task<FacultyDocument?> GetByFacultyAndCourseAsync(string facultyId, string courseId);

    Task<List<LookupDocument>> LookupAsync();

    Task<List<LookupDocument>> LookupCoursesAsync(string facultyId);

    Task DropAsync();

    Task InsertManyAsync(List<FacultyDocument> faculties);
    
    Task<bool> AnyAsync();
}