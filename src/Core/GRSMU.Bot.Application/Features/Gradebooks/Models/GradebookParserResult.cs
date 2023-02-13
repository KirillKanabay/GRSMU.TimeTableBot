using GRSMU.Bot.Domain.Gradebooks.Dtos;

namespace GRSMU.Bot.Application.Features.Gradebooks.Models;

public class GradebookParserResult
{
    public string StudentFullName { get; set; }

    public bool SignInSuccessful { get; set; }

    public GradebookDto Result { get; set; }
}