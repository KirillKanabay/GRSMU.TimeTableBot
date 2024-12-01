using GRSMU.Bot.Logic.Dtos;

namespace GRSMU.Bot.Logic.Features.Faculty.Dtos;

public record FacultyFullLookupDto(
    List<LookupDto> Faculties,
    List<LookupDto> Courses,
    List<LookupDto> Groups);