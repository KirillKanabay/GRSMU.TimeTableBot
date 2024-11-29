using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Logic.Features.Gradebook.Dtos;

namespace GRSMU.Bot.Logic.Features.Gradebook.Services.Interfaces;

public interface IGradebookProvider
{
    Task<ExecutionResult<GradebookSignInResultDto>> CheckSignInResultAsync(string login, string password);
}