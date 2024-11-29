namespace GRSMU.Bot.Web.Api.Models.User.Responses;

public record UserPrefilledFacultyModel(
    string FacultyId,
    string FacultyName,
    string CourseId,
    string CourseName,
    string GroupId,
    string GroupName);