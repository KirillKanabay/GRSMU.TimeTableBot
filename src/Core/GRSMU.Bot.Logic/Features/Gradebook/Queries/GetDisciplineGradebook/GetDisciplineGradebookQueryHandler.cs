using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Logic.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Commands.InitialFillGradebook;
using GRSMU.Bot.Logic.Features.Gradebook.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Services.Interfaces;
using GRSMU.Bot.Logic.Immutable;
using MediatR;

namespace GRSMU.Bot.Logic.Features.Gradebook.Queries.GetDisciplineGradebook;

public class GetDisciplineGradebookQueryHandler : IQueryHandler<GetDisciplineGradebookQuery, GradebookDto>
{
    private readonly ISender _sender;
    private readonly IGradebookService _gradebookService;
    private readonly IUserRepository _userRepository;

    public GetDisciplineGradebookQueryHandler(
        IGradebookService gradebookService,
        ISender sender, 
        IUserRepository userRepository)
    {
        _gradebookService = gradebookService;
        _sender = sender;
        _userRepository = userRepository;
    }

    public async Task<ExecutionResult<GradebookDto>> Handle(GetDisciplineGradebookQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);

        if (user is null)
        {
            return ExecutionResult<GradebookDto>.Failure(Error.NotFound(ErrorResourceKeys.UserNotFound));
        }

        if (string.IsNullOrWhiteSpace(user.StudentCardPassword) || string.IsNullOrWhiteSpace(user.StudentCardLogin))
        {
            return ExecutionResult<GradebookDto>.Failure(Error.ValidationError(ErrorResourceKeys.StudentCardIsNotRegistered));
        }

        var studentCard = new StudentCardIdDto(user.StudentCardLogin, user.StudentCardPassword);
        var initialFillResult = await _sender.Send(new InitialFillGradebookCommand(user.Id, studentCard), cancellationToken);

        if (initialFillResult.HasErrors)
        {
            return ExecutionResult<GradebookDto>.Failure(initialFillResult.Error!);
        }

        var gradebookSearchResult = await _gradebookService.GetUserGradebookAsync(studentCard, user.Id, request.DisciplineId, request.Force);

        return gradebookSearchResult;
    }
}