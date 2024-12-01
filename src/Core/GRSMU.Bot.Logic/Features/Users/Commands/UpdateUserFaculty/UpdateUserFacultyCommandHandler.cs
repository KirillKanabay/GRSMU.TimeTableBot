using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Data.Faculties.Contracts;
using GRSMU.Bot.Data.Faculties.Documents;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Data.Users.Documents;
using GRSMU.Bot.Logic.Immutable;

namespace GRSMU.Bot.Logic.Features.Users.Commands.UpdateUserFaculty;

public class UpdateUserFacultyCommandHandler : ICommandHandler<UpdateUserFacultyCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IFacultyRepository _facultyRepository;

    public UpdateUserFacultyCommandHandler(
        IUserRepository userRepository, 
        IFacultyRepository facultyRepository)
    {
        _userRepository = userRepository;
        _facultyRepository = facultyRepository;
    }

    public async Task<ExecutionResult> Handle(UpdateUserFacultyCommand request, CancellationToken cancellationToken)
    {
        var userDocument = await _userRepository.GetByIdAsync(request.UserId);

        if (userDocument is null)
        {
            return ExecutionResult.Failure(Error.NotFound(ErrorResourceKeys.UserNotFound));
        }

        var faculty = await _facultyRepository.GetByFacultyAndCourseAsync(request.FacultyId, request.CourseId);

        if (faculty is null)
        {
            return ExecutionResult.Failure(Error.NotFound(ErrorResourceKeys.FacultyNotFound));
        }

        var group = faculty.Groups.FirstOrDefault(x => x.GroupId.Equals(request.GroupId));

        if (group is null)
        {
            return ExecutionResult.Failure(Error.NotFound(ErrorResourceKeys.GroupNotFound));
        }

        UpdateUser(userDocument, faculty, group);

        await _userRepository.UpdateOneAsync(userDocument);

        return ExecutionResult.Success();
    }

    private void UpdateUser(UserDocument document, FacultyDocument facultyDocument, GroupDocument groupDocument)
    {
        document.FacultyId = facultyDocument.FacultyId;
        document.CourseId = facultyDocument.CourseId;
        document.GroupId = groupDocument.GroupId;
    }
}