using GRSMU.Bot.Logic.Dtos;

namespace GRSMU.Bot.Logic.Features.Faculty.Services.Interfaces;

public interface IFacultyInfoProvider
{
    Task<List<LookupDto>> GetFacultyLookupAsync();

    Task<List<LookupDto>> GetCoursesLookupAsync();

    Task<List<LookupDto>> GetGroupsLookupAsync(string facultyId, string courseId);
}