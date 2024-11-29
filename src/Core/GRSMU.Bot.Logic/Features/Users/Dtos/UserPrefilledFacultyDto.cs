namespace GRSMU.Bot.Logic.Features.Users.Dtos;

public record UserPrefilledFacultyDto(
    string FacultyId,
    string FacultyName,
    string CourseId,
    string CourseName,
    string GroupId,
    string GroupName);