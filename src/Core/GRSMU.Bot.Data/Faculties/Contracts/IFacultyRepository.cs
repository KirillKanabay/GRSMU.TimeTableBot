using GRSMU.Bot.Data.Common.Documents;
using GRSMU.Bot.Data.Faculties.Documents;

namespace GRSMU.Bot.Data.Faculties.Contracts;

public interface IFacultyRepository
{
    Task<List<FacultyDocument>> SearchByFacultyId(string facultyId);

    Task<FacultyDocument?> GetByFacultyAndCourse(string facultyId, string courseId);

    Task<List<LookupDocument>> LookupAsync();

    Task DropAsync();

    Task InsertManyAsync(List<FacultyDocument> faculties);
    
    Task<bool> AnyAsync();
}