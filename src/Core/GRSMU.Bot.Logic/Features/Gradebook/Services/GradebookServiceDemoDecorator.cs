using GRSMU.Bot.Common.Enums;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Data.Gradebooks.Contracts;
using GRSMU.Bot.Data.Gradebooks.Documents;
using GRSMU.Bot.Logic.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Services.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace GRSMU.Bot.Logic.Features.Gradebook.Services;

public class GradebookServiceDemoDecorator : IGradebookService
{
    private const string DemoStudentIdLogin = "demouser";
    private const string DemoStudentIdPassword = "99-99999";

    private readonly IGradebookService _gradebookService;
    private readonly ILogger<GradebookServiceDemoDecorator> _logger;
    private readonly IGradebookRepository _gradebookRepository;

    public GradebookServiceDemoDecorator(
        IGradebookService gradebookService,
        ILogger<GradebookServiceDemoDecorator> logger,
        IGradebookRepository gradebookRepository)
    {
        _gradebookService = gradebookService;
        _logger = logger;
        _gradebookRepository = gradebookRepository;
    }

    public async Task<ExecutionResult<GradebookSignInResultDto>> SignInAsync(StudentCardIdDto studentCardId)
    {
        if (studentCardId is { Login: DemoStudentIdLogin, Password: DemoStudentIdPassword })
        {
            _logger.LogInformation("Demo user sign in");

            return ExecutionResult.Success(
                new GradebookSignInResultDto("Демонстрационный пользователь", "demo", "demo"));
        }

        return await _gradebookService.SignInAsync(studentCardId);
    }

    public Task<ExecutionResult<GradebookDto>> GetUserGradebookAsync(StudentCardIdDto studentCardId, string userId, string disciplineId, bool forceRefresh)
    {
        return _gradebookService.GetUserGradebookAsync(studentCardId, userId, disciplineId, forceRefresh);
    }

    public Task<ExecutionResult<List<LookupDto>>> GetDisciplineLookupAsync(string userId, string searchQuery)
    {
        return _gradebookService.GetDisciplineLookupAsync(userId, searchQuery);
    }

    public async Task<ExecutionResult> UpdateUserGradebook(StudentCardIdDto studentCardId, string userId)
    {
        if (studentCardId is { Login: DemoStudentIdLogin, Password: DemoStudentIdPassword })
        {
            _logger.LogInformation("Requested gradebook update for demo user");
            var documents = GetDemoUserGradebook(userId);
            await _gradebookRepository.UpdateManyAsync(documents);
            return ExecutionResult.Success();
        }

        return await _gradebookService.UpdateUserGradebook(studentCardId, userId);
    }

    private List<GradebookDocument> GetDemoUserGradebook (string userId)
    {
        var userObjId = ObjectId.Parse(userId);
        return
        [
            new GradebookDocument
            {
                Discipline = "All activity types",
                UserId = userObjId,
                Marks = [
                    new()
                    {
                        ActivityType = MarkActivityType.Unknown,
                        Date = DateTime.UtcNow.Date.AddDays(-11).ToString(),
                        ParsedDate = DateTime.UtcNow.Date.AddDays(-11),
                        Mark = "",
                        Type = MarkType.None,
                    },
                    new()
                    {
                        ActivityType = MarkActivityType.Practise,
                        Date = DateTime.UtcNow.Date.AddDays(-10).ToString(),
                        ParsedDate = DateTime.UtcNow.Date.AddDays(-10),
                        Mark = "",
                        Type = MarkType.None,
                    },
                    new()
                    {
                        ActivityType = MarkActivityType.Final,
                        Date = DateTime.UtcNow.Date.AddDays(-9).ToString(),
                        ParsedDate = DateTime.UtcNow.Date.AddDays(-9),
                        Mark = "10",
                        Type = MarkType.DefaultMark,
                    },
                    new()
                    {
                        ActivityType = MarkActivityType.Test,
                        Date = DateTime.UtcNow.Date.AddDays(-8).ToString(),
                        ParsedDate = DateTime.UtcNow.Date.AddDays(-8),
                        Mark = "",
                        Type = MarkType.None,
                    },
                    new()
                    {
                        ActivityType = MarkActivityType.HistoryOfDiseases,
                        Date = DateTime.UtcNow.Date.AddDays(-7).ToString(),
                        ParsedDate = DateTime.UtcNow.Date.AddDays(-7),
                        Mark = "5",
                        Type = MarkType.DefaultMark,
                    },
                    new()
                    {
                        ActivityType = MarkActivityType.DifferentiatedExam,
                        Date = DateTime.UtcNow.Date.AddDays(-6).ToString(),
                        ParsedDate = DateTime.UtcNow.Date.AddDays(-6),
                        Mark = "5",
                        Type = MarkType.NotSeriousWorkOutMark,
                    },
                    new()
                    {
                        ActivityType = MarkActivityType.Exam,
                        Date = DateTime.UtcNow.Date.AddDays(-5).ToString(),
                        ParsedDate = DateTime.UtcNow.Date.AddDays(-5),
                        Mark = "10",
                        Type = MarkType.SeriousWorkOutMark,
                    },
                    new()
                    {
                        ActivityType = MarkActivityType.Seminar,
                        Date = DateTime.UtcNow.Date.AddDays(-4).ToString(),
                        ParsedDate = DateTime.UtcNow.Date.AddDays(-4),
                        Mark = "н",
                        Type = MarkType.UnknownAbsence,
                    },
                    new()
                    {
                        ActivityType = MarkActivityType.Lecture,
                        Date = DateTime.UtcNow.Date.AddDays(-3).ToString(),
                        ParsedDate = DateTime.UtcNow.Date.AddDays(-3),
                        Mark = "зач",
                        Type = MarkType.LecturePassed,
                    },
                    new()
                    {
                        ActivityType = MarkActivityType.Lecture,
                        Date = DateTime.UtcNow.Date.AddDays(-2).ToString(),
                        ParsedDate = DateTime.UtcNow.Date.AddDays(-2),
                        Mark = "нзч",
                        Type = MarkType.LectureFail,
                    },
                    new()
                    {
                        ActivityType = MarkActivityType.Training,
                        Date = DateTime.UtcNow.Date.AddDays(-2).ToString(),
                        ParsedDate = DateTime.UtcNow.Date.AddDays(-2),
                        Mark = "нбп",
                        Type = MarkType.SeriousAbsenceWithoutRepeatVisit,
                    },
                    new()
                    {
                        ActivityType = MarkActivityType.PractiseSkills,
                        Date = DateTime.UtcNow.Date.AddDays(-2).ToString(),
                        ParsedDate = DateTime.UtcNow.Date.AddDays(-2),
                        Mark = "нн",
                        Type = MarkType.NotSeriousAbsence,
                    },
                    new()
                    {
                        ActivityType = MarkActivityType.Lecture,
                        Date = DateTime.UtcNow.Date.AddDays(-2).ToString(),
                        ParsedDate = DateTime.UtcNow.Date.AddDays(-2),
                        Mark = "нy",
                        Type = MarkType.SeriousAbsence,
                    },
                    new()
                    {
                        ActivityType = MarkActivityType.Lecture,
                        Date = DateTime.UtcNow.Date.AddDays(-2).ToString(),
                        ParsedDate = DateTime.UtcNow.Date.AddDays(-2),
                        Mark = "33",
                        Type = MarkType.NotSeriousWorkOutMark,
                    },
                ],
                ExamMark = "10.0",
                TotalAverageMark = "5.55",
            }
        ];
    }
}