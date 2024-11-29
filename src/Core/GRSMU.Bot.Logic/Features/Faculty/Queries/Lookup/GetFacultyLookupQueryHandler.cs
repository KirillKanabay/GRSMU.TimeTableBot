using AutoMapper;
using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Data.Faculties.Contracts;
using GRSMU.Bot.Logic.Dtos;

namespace GRSMU.Bot.Logic.Features.Faculty.Queries.Lookup;

public class GetFacultyLookupQueryHandler : IQueryHandler<GetFacultyLookupQuery, List<LookupDto>>
{
    private readonly IFacultyRepository _facultyRepository;
    private readonly IMapper _mapper;

    public GetFacultyLookupQueryHandler(IFacultyRepository facultyRepository, IMapper mapper)
    {
        _facultyRepository = facultyRepository;
        _mapper = mapper;
    }

    public async Task<ExecutionResult<List<LookupDto>>> Handle(GetFacultyLookupQuery request, CancellationToken cancellationToken)
    {
        var facultyLookupResult = await _facultyRepository.LookupAsync();

        var dtos = _mapper.Map<List<LookupDto>>(facultyLookupResult);

        return ExecutionResult.Success(dtos);
    }
}