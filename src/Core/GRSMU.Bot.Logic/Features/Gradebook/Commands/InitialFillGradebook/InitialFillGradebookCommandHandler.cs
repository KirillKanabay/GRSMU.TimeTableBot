using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Data.Gradebooks.Contracts;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Logic.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Services.Interfaces;
using GRSMU.Bot.Logic.Features.Users.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GRSMU.Bot.Logic.Features.Gradebook.Commands.InitialFillGradebook;

public class InitialFillGradebookCommandHandler : ICommandHandler<InitialFillGradebookCommand, InitialFillGradebookResultDto>, INotificationHandler<StudentCardUpdatedEvent>
{
    private readonly IGradebookRepository _gradebookRepository;
    private readonly IGradebookService _gradebookService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<InitialFillGradebookCommandHandler> _logger;

    public InitialFillGradebookCommandHandler(
        IGradebookService gradebookService,
        IUserRepository userRepository, 
        IGradebookRepository gradebookRepository,
        ILogger<InitialFillGradebookCommandHandler> logger)
    {
        _gradebookService = gradebookService;
        _userRepository = userRepository;
        _gradebookRepository = gradebookRepository;
        _logger = logger;
    }

    public Task<ExecutionResult<InitialFillGradebookResultDto>> Handle(InitialFillGradebookCommand request, CancellationToken cancellationToken)
    {
        return FillGradebookInternal(request.UserId, request.StudentCard);
    }

    public Task Handle(StudentCardUpdatedEvent notification, CancellationToken cancellationToken)
    {
        return FillGradebookInternal(notification.UserId, notification.StudentCard, true);
    }

    private async Task<ExecutionResult<InitialFillGradebookResultDto>> FillGradebookInternal(string userId, StudentCardIdDto studentCard, bool force = false)
    {
        if (!force)
        {
            var gradebookExists = await _gradebookRepository.AnyAsync(userId);

            if (gradebookExists)
            {
                return ExecutionResult.Success(new InitialFillGradebookResultDto(true));
            }
        }

        _logger.LogInformation($"Requested gradebook initial fill for userId: {userId}");

        await _gradebookRepository.DeleteGradebookByUserAsync(userId);
        var result = await _gradebookService.UpdateUserGradebook(studentCard, userId);

        if (result.HasErrors)
        {
            return ExecutionResult<InitialFillGradebookResultDto>.Failure(result.Error!);
        }

        return ExecutionResult.Success(new InitialFillGradebookResultDto(false));
    }
}
