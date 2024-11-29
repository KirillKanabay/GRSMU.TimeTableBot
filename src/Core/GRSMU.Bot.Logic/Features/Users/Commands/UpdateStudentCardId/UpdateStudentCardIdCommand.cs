using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Logic.Features.Users.Dtos;

namespace GRSMU.Bot.Logic.Features.Users.Commands.UpdateStudentCardId;

public record UpdateStudentCardIdCommand(
    string UserId,
    string Login,
    string Password,
    string FacultyId) : ICommand<UserPrefilledFacultyDto>;