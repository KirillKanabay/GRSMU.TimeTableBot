using FluentValidation;

namespace GRSMU.Bot.Logic.Features.Gradebook.Queries.GetDisciplineGradebook;

public class GetDisciplineGradebookQueryValidator : AbstractValidator<GetDisciplineGradebookQuery>
{
    public GetDisciplineGradebookQueryValidator()
    {
        RuleFor(x => x.UserId).NotNull().NotEmpty();
        RuleFor(x => x.DisciplineId).NotNull().NotEmpty();
    }
}