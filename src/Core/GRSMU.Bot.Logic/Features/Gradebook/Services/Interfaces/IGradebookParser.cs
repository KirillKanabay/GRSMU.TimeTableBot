using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Logic.Features.Gradebook.Dtos;

namespace GRSMU.Bot.Logic.Features.Gradebook.Services.Interfaces;

public interface IGradebookParser
{
    Task<ExecutionResult<GradebookSignInResultDto>> ParseSignInResultAsync(string rawPage);

    Task<ExecutionResult<List<GradebookDto>>> ParseGradebookAsync(string rawPage);
}