using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Logic.Features.Gradebook.Dtos;


namespace GRSMU.Bot.Logic.Features.Gradebook.Queries.GetDisciplineGradebook;

public record GetDisciplineGradebookQuery(
    string UserId,
    string DisciplineId,
    bool Force) : IQuery<GradebookDto>;