using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Logic.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace GRSMU.Bot.Logic.Features.Gradebook.Services;

public class GradebookServiceDemoDecorator : IGradebookService
{
    private const string DemoStudentIdLogin = "demouser";
    private const string DemoStudentIdPassword = "99-99999";

    private readonly IGradebookService _gradebookService;
    private readonly ILogger<GradebookServiceDemoDecorator> _logger;

    public GradebookServiceDemoDecorator(
        IGradebookService gradebookService,
        ILogger<GradebookServiceDemoDecorator> logger)
    {
        _gradebookService = gradebookService;
        _logger = logger;
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
}