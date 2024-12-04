using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Logic.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Commands.InitialFillGradebook;
using GRSMU.Bot.Logic.Features.Gradebook.Services.Interfaces;
using GRSMU.Bot.Logic.Immutable;
using MediatR;

namespace GRSMU.Bot.Logic.Features.Gradebook.Queries.GetDisciplineLookup;

public class GetDisciplineLookupQueryHandler : IQueryHandler<GetDisciplineLookupQuery, List<LookupDto>>
{
    private readonly ISender _sender;
    private readonly IUserRepository _userRepository;
    private readonly IGradebookService _gradebookService;

    public GetDisciplineLookupQueryHandler(
        ISender sender, 
        IUserRepository userRepository, 
        IGradebookService gradebookService)
    {
        _sender = sender;
        _userRepository = userRepository;
        _gradebookService = gradebookService;
    }

    public async Task<ExecutionResult<List<LookupDto>>> Handle(GetDisciplineLookupQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);

        if (user is null)
        {
            return ExecutionResult<List<LookupDto>>.Failure(Error.NotFound(ErrorResourceKeys.UserNotFound));
        }

        if (string.IsNullOrWhiteSpace(user.StudentCardPassword) || string.IsNullOrWhiteSpace(user.StudentCardLogin))
        {
            return ExecutionResult<List<LookupDto>>.Failure(Error.ValidationError(ErrorResourceKeys.StudentCardIsNotRegistered));
        }

        var studentCard = new StudentCardIdDto(user.StudentCardLogin, user.StudentCardPassword);
        var initialFillResult = await _sender.Send(new InitialFillGradebookCommand(user.Id, studentCard), cancellationToken);

        if (initialFillResult.HasErrors)
        {
            return ExecutionResult<List<LookupDto>>.Failure(initialFillResult.Error!);
        }

        var lookupResult = await _gradebookService.GetDisciplineLookupAsync(request.UserId, request.SearchQuery);

        return lookupResult;
    }
}