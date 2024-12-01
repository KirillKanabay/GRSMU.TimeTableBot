namespace GRSMU.Bot.Web.Api.Models.User.Requests;

public record UpdateStudentFacultyRequest(
    string FacultyId,
    string CourseId,
    string GroupId);