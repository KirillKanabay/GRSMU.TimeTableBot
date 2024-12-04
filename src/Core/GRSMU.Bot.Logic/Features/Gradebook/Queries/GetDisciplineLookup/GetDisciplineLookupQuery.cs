using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Logic.Dtos;

namespace GRSMU.Bot.Logic.Features.Gradebook.Queries.GetDisciplineLookup;

public record GetDisciplineLookupQuery(
    string UserId,
    string? SearchQuery) : IQuery<List<LookupDto>>;