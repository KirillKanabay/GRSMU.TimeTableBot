using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Logic.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Dtos;

namespace GRSMU.Bot.Logic.Features.Gradebook.Services.Interfaces;

public interface IGradebookProvider
{
    Task<ExecutionResult<GradebookSignInResultDto>> CheckSignInResultAsync(StudentCardIdDto studentCardId);

    Task<ExecutionResult<List<GradebookDto>>> GetUserGradebookAsync(StudentCardIdDto studentCardId);
}