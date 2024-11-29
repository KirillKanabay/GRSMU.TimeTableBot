using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Logic.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Services.Interfaces;

namespace GRSMU.Bot.Logic.Features.Gradebook.Services
{
    public class GradebookService : IGradebookService
    {
        private readonly IGradebookProvider _gradebookProvider;

        public GradebookService(IGradebookProvider gradebookProvider)
        {
            _gradebookProvider = gradebookProvider;
        }

        public Task<ExecutionResult<GradebookSignInResultDto>> SignInAsync(StudentCardIdDto studentCardId)
        {
            return _gradebookProvider.CheckSignInResultAsync(studentCardId.Login, studentCardId.Password);
        }
    }
}
