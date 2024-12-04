using FluentValidation;

namespace GRSMU.Bot.Logic.Features.Gradebook.Commands.InitialFillGradebook
{
    internal class InitialFillGradebookCommandValidator : AbstractValidator<InitialFillGradebookCommand>
    {
        public InitialFillGradebookCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
        }
    }
}
