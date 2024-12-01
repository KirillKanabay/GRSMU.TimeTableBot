using AutoMapper;
using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Data.Faculties.Contracts;
using GRSMU.Bot.Logic.Dtos;
using GRSMU.Bot.Logic.Features.Faculty.Dtos;
using GRSMU.Bot.Logic.Immutable;

namespace GRSMU.Bot.Logic.Features.Faculty.Queries.FullLookup;

public class GetFacultyFullLookupQueryHandler : IQueryHandler<GetFacultyFullLookupQuery, FacultyFullLookupDto>
{
    private readonly IMapper _mapper;
    private readonly IFacultyRepository _facultyRepository;

    public GetFacultyFullLookupQueryHandler(
        IMapper mapper,
        IFacultyRepository facultyRepository)
    {
        _mapper = mapper;
        _facultyRepository = facultyRepository;
    }

    public async Task<ExecutionResult<FacultyFullLookupDto>> Handle(GetFacultyFullLookupQuery request, CancellationToken cancellationToken)
    {
        var faculty = await _facultyRepository.GetByFacultyAndCourseAsync(request.FacultyId, request.CourseId);

        if (faculty is null)
        {
            return ExecutionResult.Failure<FacultyFullLookupDto>(Error.NotFound(ErrorResourceKeys.FacultyNotFound));
        }

        var coursesLookup = await _facultyRepository.LookupCoursesAsync(faculty.FacultyId);
        var allFacultiesLookup = await _facultyRepository.LookupAsync();

        var fullLookupDto = new FacultyFullLookupDto(
            _mapper.Map<List<LookupDto>>(allFacultiesLookup),
            _mapper.Map<List<LookupDto>>(coursesLookup),
            _mapper.Map<List<LookupDto>>(faculty.Groups));

        return ExecutionResult.Success(fullLookupDto);
    }
}