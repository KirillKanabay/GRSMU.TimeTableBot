using GRSMU.Bot.Logic.Dtos;

namespace GRSMU.Bot.Web.Api.Models.Faculty.Responses;

public record FacultyFullLookupResponse(
    List<LookupDto> Faculties,
    List<LookupDto> Courses,
    List<LookupDto> Groups);
