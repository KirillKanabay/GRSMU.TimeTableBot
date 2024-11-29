using AutoMapper;
using GRSMU.Bot.Logic.Features.Faculty.Queries.Lookup;
using GRSMU.Bot.Web.Api.Extensions;
using GRSMU.Bot.Web.Api.Models;
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
    public async Task<ActionResult<List<LookupModel>>> GetFacultyLookup()
    {
        var lookupResult = await _sender.Send(new GetFacultyLookupQuery());

        if (lookupResult.HasErrors)
        {
            return lookupResult.ToFailureActionResult();
        }

        var dtoModels = _mapper.Map<List<LookupModel>>(lookupResult.Data);

        return Ok(dtoModels);
    }
}