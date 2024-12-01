using FluentValidation;

namespace GRSMU.Bot.Logic.Features.Faculty.Queries.FullLookup;

public class GetFacultyFullLookupQueryValidator : AbstractValidator<GetFacultyFullLookupQuery>
{
    public GetFacultyFullLookupQueryValidator()
    {
        RuleFor(x => x.CourseId).NotNull().NotEmpty();
        RuleFor(x => x.FacultyId).NotNull().NotEmpty();
    }
}