using AutoMapper;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Data.Gradebooks.Contracts;
using GRSMU.Bot.Data.Gradebooks.Documents;
using GRSMU.Bot.Logic.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Services.Interfaces;
using GRSMU.Bot.Logic.Immutable;

namespace GRSMU.Bot.Logic.Features.Gradebook.Services;

public class GradebookService : IGradebookService
{
    private readonly IGradebookProvider _gradebookProvider;
    private readonly IGradebookRepository _gradebookRepository;
    private readonly IMapper _mapper;

    public GradebookService(
        IGradebookProvider gradebookProvider, 
        IGradebookRepository gradebookRepository,
        IMapper mapper)
    {
        _gradebookProvider = gradebookProvider;
        _gradebookRepository = gradebookRepository;
        _mapper = mapper;
    }

    public Task<ExecutionResult<GradebookSignInResultDto>> SignInAsync(StudentCardIdDto studentCardId)
    {
        return _gradebookProvider.CheckSignInResultAsync(studentCardId);
    }
    
    public async Task<ExecutionResult<GradebookDto>> GetUserGradebookAsync(StudentCardIdDto studentCardId, string userId, string disciplineId, bool forceRefresh)
    {
        var gradebook = await _gradebookRepository.GetByUserAndDisciplineAsync(userId, disciplineId);

        if (gradebook is null)
        {
            return ExecutionResult<GradebookDto>.Failure(Error.NotFound(ErrorResourceKeys.GradebookNotFound));
        }

        if (forceRefresh && gradebook.CreatedDate.HasValue && (DateTime.UtcNow - gradebook.CreatedDate.Value).TotalMinutes > 30)
        {
            var fetchResult = await UpdateUserGradebook(studentCardId, userId);

            if (fetchResult.HasErrors)
            {
                return ExecutionResult<GradebookDto>.Failure(fetchResult.Error!);
            }

            return await GetUserGradebookAsync(studentCardId, userId, disciplineId, false);
        }

        var dto = _mapper.Map<GradebookDto>(gradebook);

        return ExecutionResult.Success(dto);
    }

    public async Task<ExecutionResult<List<LookupDto>>> GetDisciplineLookupAsync(string userId, string searchQuery)
    {
        var lookup = await _gradebookRepository.GetDisciplineLookup(userId, searchQuery);
        var dtos = _mapper.Map<List<LookupDto>>(lookup);

        return ExecutionResult.Success(dtos);
    }

    public async Task<ExecutionResult> UpdateUserGradebook(StudentCardIdDto studentCardId, string userId)
    {
        var gradebookParseResult = await _gradebookProvider.GetUserGradebookAsync(studentCardId);

        if (gradebookParseResult.HasErrors)
        {
            return ExecutionResult.Failure(gradebookParseResult.Error!);
        }

        var gradebooks = gradebookParseResult.Data;
        gradebooks.ForEach(x => x.UserId = userId);

        var documents = _mapper.Map<List<GradebookDocument>>(gradebooks);
        await _gradebookRepository.UpdateManyAsync(documents);

        return ExecutionResult.Success();
    }
}
