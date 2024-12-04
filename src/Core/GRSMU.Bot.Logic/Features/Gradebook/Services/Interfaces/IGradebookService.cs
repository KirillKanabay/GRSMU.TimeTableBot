using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Logic.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Dtos;

namespace GRSMU.Bot.Logic.Features.Gradebook.Services.Interfaces
{
    public interface IGradebookService
    {
        Task<ExecutionResult<GradebookSignInResultDto>> SignInAsync(StudentCardIdDto studentCardId);

        Task<ExecutionResult<GradebookDto>> GetUserGradebookAsync(StudentCardIdDto studentCardId, string userId, string disciplineId, 
            bool forceRefresh);

        Task<ExecutionResult<List<LookupDto>>> GetDisciplineLookupAsync(string userId, string searchQuery);

        Task<ExecutionResult> UpdateUserGradebook(StudentCardIdDto studentCardId, string userId);
    }
}
