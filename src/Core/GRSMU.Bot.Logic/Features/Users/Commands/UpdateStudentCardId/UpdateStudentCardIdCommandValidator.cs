using System.Text.RegularExpressions;
using FluentValidation;

namespace GRSMU.Bot.Logic.Features.Users.Commands.UpdateStudentCardId;

public class UpdateStudentCardIdCommandValidator : AbstractValidator<UpdateStudentCardIdCommand>
{
    public UpdateStudentCardIdCommandValidator()
    {
        RuleFor(x => x.Login).NotNull().NotEmpty();
        RuleFor(x => x.Password).NotNull().NotEmpty().Matches(new Regex(@"^\d{2}-\d{5}$"));
        RuleFor(x => x.FacultyId).NotNull().NotEmpty();
    }
}