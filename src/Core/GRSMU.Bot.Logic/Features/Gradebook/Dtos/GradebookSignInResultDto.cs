namespace GRSMU.Bot.Logic.Features.Gradebook.Dtos;

public record GradebookSignInResultDto(
    string StudentFullName,
    int StudentCourseNumber,
    int StudentGroupNumber);
