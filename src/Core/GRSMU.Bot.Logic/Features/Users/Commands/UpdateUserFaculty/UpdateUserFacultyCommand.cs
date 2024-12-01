using GRSMU.Bot.Common.Messaging;

namespace GRSMU.Bot.Logic.Features.Users.Commands.UpdateUserFaculty;

public record UpdateUserFacultyCommand(
    string UserId,
    string FacultyId,
    string CourseId,
    string GroupId) : ICommand;