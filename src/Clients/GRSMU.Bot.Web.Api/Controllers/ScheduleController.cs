using AutoMapper;
using GRSMU.Bot.Common.Resources;
using GRSMU.Bot.Logic.Features.Schedule.Queries.GetByDate;
using GRSMU.Bot.Web.Api.Extensions;
using GRSMU.Bot.Web.Api.Models.Schedule.Responses;
using GRSMU.Bot.Web.Core.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GRSMU.Bot.Web.Api.Controllers;

[ApiController]
[Route("api/schedule")]
[Authorize]
public class ScheduleController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;
    private readonly IResourceProvider _resourceProvider;

    public ScheduleController(
        ISender sender, 
        IMapper mapper, 
        IResourceProvider resourceProvider)
    {
        _sender = sender;
        _mapper = mapper;
        _resourceProvider = resourceProvider;
    }

    [HttpGet("by-date")]
    public async Task<ActionResult<ScheduleDayModel>> GetByDateAsync(DateTime date, bool forceRefresh = false)
    {
        var userId = HttpContext.User.GetId();

        var request = new GetScheduleByDateQuery(date, userId, forceRefresh);
        var result = await _sender.Send(request);

        if (result.HasErrors)
        {
            return result.ToFailureActionResult(_resourceProvider);
        }

        var model = _mapper.Map<ScheduleDayModel>(result.Data);

        return Ok(model);
    }
}