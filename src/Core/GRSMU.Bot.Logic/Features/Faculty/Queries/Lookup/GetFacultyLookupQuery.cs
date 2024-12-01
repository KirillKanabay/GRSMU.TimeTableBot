using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Logic.Dtos;

namespace GRSMU.Bot.Logic.Features.Faculty.Queries.Lookup;

public record GetFacultyLookupQuery : IQuery<List<LookupDto>>;