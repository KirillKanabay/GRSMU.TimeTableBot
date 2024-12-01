using System.Text.RegularExpressions;
using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Data.Faculties.Contracts;
using GRSMU.Bot.Data.Faculties.Documents;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Logic.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Services.Interfaces;
using GRSMU.Bot.Logic.Features.Users.Dtos;
using GRSMU.Bot.Logic.Immutable;
using Microsoft.Extensions.Logging;

namespace GRSMU.Bot.Logic.Features.Users.Commands.UpdateStudentCardId;

public class UpdateStudentCardIdCommandHandler : ICommandHandler<UpdateStudentCardIdCommand, UserPrefilledFacultyDto>
{
    private readonly IGradebookService _gradebookService;
    private readonly IUserRepository _userRepository;
    private readonly IFacultyRepository _facultyRepository;
    private readonly ILogger<UpdateStudentCardIdCommandHandler> _logger;
     
    public UpdateStudentCardIdCommandHandler(
        IGradebookService gradebookService,
        IUserRepository userRepository,
        IFacultyRepository facultyRepository,
        ILogger<UpdateStudentCardIdCommandHandler> logger)
    {
        _gradebookService = gradebookService;
        _userRepository = userRepository;
        _facultyRepository = facultyRepository;
        _logger = logger;
    }

    public async Task<ExecutionResult<UserPrefilledFacultyDto>> Handle(UpdateStudentCardIdCommand request, CancellationToken cancellationToken)
    {
        var userDocument = await _userRepository.GetByIdAsync(request.UserId);

        if (userDocument is null)
        {
            return ExecutionResult<UserPrefilledFacultyDto>.Failure(Error.NotFound(ErrorResourceKeys.UserNotFound));
        }

        var signInResult = await _gradebookService.SignInAsync(new StudentCardIdDto(request.Login, request.Password));

        if (signInResult.HasErrors)
        {
            return ExecutionResult<UserPrefilledFacultyDto>.Failure(signInResult.Error!);
        }

        var prefillResult = await GetPrefilledData(request.FacultyId, signInResult.Data);

        if (prefillResult.HasErrors)
        {
            return ExecutionResult<UserPrefilledFacultyDto>.Failure(prefillResult.Error!);
        }

        userDocument.StudentCardLogin = request.Login;
        userDocument.StudentCardPassword = request.Password;

        await _userRepository.UpdateOneAsync(userDocument);

        return ExecutionResult.Success(prefillResult.Data);
    }

    private async Task<ExecutionResult<UserPrefilledFacultyDto>> GetPrefilledData(string facultyId, GradebookSignInResultDto signInResult)
    {
        var faculty = await _facultyRepository.GetByFacultyAndCourseAsync(facultyId, signInResult.StudentCourseNumber);
        
        if (faculty is null)
        {
            var faculties = await _facultyRepository.SearchByFacultyIdAsync(facultyId);

            if (!faculties.Any())
            {
                _logger.LogError($"Faculty not found: {facultyId}");
                return ExecutionResult<UserPrefilledFacultyDto>.Failure(Error.Problem(ErrorResourceKeys.FacultyNotFilled));
            }

            faculty = faculties.First();

            if (!faculty.Groups.Any())
            {
                _logger.LogError($"Faculty:{facultyId} doesn't have groups");
                return ExecutionResult<UserPrefilledFacultyDto>.Failure(Error.Problem(ErrorResourceKeys.FacultyNotFilled));
            }

            return ExecutionResult.Success(CreateDto(faculty, faculty.Groups.First()));
        }

        if (!faculty.Groups.Any())
        {
            _logger.LogError($"Faculty:{facultyId} doesn't have groups");
            return ExecutionResult<UserPrefilledFacultyDto>.Failure(Error.Problem(ErrorResourceKeys.FacultyNotFilled));
        }

        var group = int.TryParse(signInResult.GroupNumber, out var groupInt)
                ? faculty.Groups.FirstOrDefault(g => Regex.IsMatch(g.GroupName, $@"\b{groupInt:D2}\w+\b"))
                : faculty.Groups.First();

        group ??= faculty.Groups.First();

        return ExecutionResult.Success(CreateDto(faculty, group));

        UserPrefilledFacultyDto CreateDto(FacultyDocument facultyDocument, GroupDocument groupDocument)
        {
            return new UserPrefilledFacultyDto(
                facultyDocument.FacultyId,
                facultyDocument.FacultyName,
                facultyDocument.CourseId,
                facultyDocument.CourseName,
                groupDocument.GroupId,
                groupDocument.GroupName);
        }
    }
}