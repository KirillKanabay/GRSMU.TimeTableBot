using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Logic.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Services.Interfaces;

namespace GRSMU.Bot.Logic.Features.Gradebook.Services
{
    public class GradebookService : IGradebookService
    {
        public Task<ExecutionResult<GradebookSignInResultDto>> SignInAsync(StudentCardIdDto studentCardId)
        {
            throw new NotImplementedException();
        }
    }
}
