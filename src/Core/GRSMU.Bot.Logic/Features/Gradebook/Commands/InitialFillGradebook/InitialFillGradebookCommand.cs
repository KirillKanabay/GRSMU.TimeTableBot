using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Logic.Dtos;

namespace GRSMU.Bot.Logic.Features.Gradebook.Commands.InitialFillGradebook;

public record InitialFillGradebookCommand(string UserId, StudentCardIdDto StudentCard) : ICommand<InitialFillGradebookResultDto>;