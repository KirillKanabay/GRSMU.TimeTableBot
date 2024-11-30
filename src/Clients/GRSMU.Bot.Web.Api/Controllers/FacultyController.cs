using AutoMapper;
using GRSMU.Bot.Logic.Features.Faculty.Queries.FullLookup;
using GRSMU.Bot.Logic.Features.Faculty.Queries.Lookup;
using GRSMU.Bot.Web.Api.Extensions;
using GRSMU.Bot.Web.Api.Models;
using GRSMU.Bot.Web.Api.Models.Faculty.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GRSMU.Bot.Web.Api.Controllers;

[ApiController]
[Route("api/faculty")]
[Authorize]
public class FacultyController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public FacultyController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet("lookup")]
    public async Task<ActionResult<List<LookupModel>>> GetFacultyLookupAsync()
    {
        var lookupResult = await _sender.Send(new GetFacultyLookupQuery());

        if (lookupResult.HasErrors)
        {
            return lookupResult.ToFailureActionResult();
        }

        var models = _mapper.Map<List<LookupModel>>(lookupResult.Data);

        return Ok(models);
    }

    [HttpGet("full-lookup")]
    public async Task<ActionResult<FacultyFullLookupResponse>> GetFacultyFullLookupAsync(string facultyId,
        string courseId)
    {
        var lookupResult = await _sender.Send(new GetFacultyFullLookupQuery(facultyId, courseId));
        
        if (lookupResult.HasErrors)
        {
            return lookupResult.ToFailureActionResult();
        }

        var response = _mapper.Map<FacultyFullLookupResponse>(lookupResult.Data);

        return Ok(response);
    }
}