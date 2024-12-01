using FluentValidation;

namespace GRSMU.Bot.Logic.Features.Users.Commands.UpdateUserFaculty;

public class UpdateUserFacultyCommandValidator : AbstractValidator<UpdateUserFacultyCommand>
{
    public UpdateUserFacultyCommandValidator()
    {
        RuleFor(x => x.UserId).NotNull().NotEmpty();
        RuleFor(x => x.CourseId).NotNull().NotEmpty();
        RuleFor(x => x.GroupId).NotNull().NotEmpty();
        RuleFor(x => x.FacultyId).NotNull().NotEmpty();
    }
}