using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Logic.Features.Faculty.Dtos;

namespace GRSMU.Bot.Logic.Features.Faculty.Queries.FullLookup;

public record GetFacultyFullLookupQuery(
    string FacultyId,
    string CourseId
) : IQuery<FacultyFullLookupDto>;