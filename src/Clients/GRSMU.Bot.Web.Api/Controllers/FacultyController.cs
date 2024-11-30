using AutoMapper;
using GRSMU.Bot.Common.Resources;
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
    private readonly IResourceProvider _resourceProvider;

    public FacultyController(
        ISender sender,
        IMapper mapper,
        IResourceProvider resourceProvider)
    {
        _sender = sender;
        _mapper = mapper;
        _resourceProvider = resourceProvider;
    }

    [HttpGet("lookup")]
    public async Task<ActionResult<List<LookupModel>>> GetFacultyLookupAsync()
    {
        var lookupResult = await _sender.Send(new GetFacultyLookupQuery());

        if (lookupResult.HasErrors)
        {
            return lookupResult.ToFailureActionResult(_resourceProvider);
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
            return lookupResult.ToFailureActionResult(_resourceProvider);
        }

        var response = _mapper.Map<FacultyFullLookupResponse>(lookupResult.Data);

        return Ok(response);
    }
}