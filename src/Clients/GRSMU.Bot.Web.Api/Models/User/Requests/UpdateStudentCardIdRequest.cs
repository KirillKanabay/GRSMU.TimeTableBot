namespace GRSMU.Bot.Web.Api.Models.User.Requests;

public record UpdateStudentCardIdRequest(
    string Login,
    string Password,
    string FacultyId);