using FluentValidation;

namespace GRSMU.Bot.Logic.Features.Gradebook.Queries.GetDisciplineLookup;

public class GetDisciplineLookupQueryValidator : AbstractValidator<GetDisciplineLookupQuery>
{
    public GetDisciplineLookupQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().NotNull();
    }
}