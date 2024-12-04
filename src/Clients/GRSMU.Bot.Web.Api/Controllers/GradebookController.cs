using AutoMapper;
using GRSMU.Bot.Common.Resources;
using GRSMU.Bot.Logic.Features.Gradebook.Queries.GetDisciplineGradebook;
using GRSMU.Bot.Logic.Features.Gradebook.Queries.GetDisciplineLookup;
using GRSMU.Bot.Web.Api.Extensions;
using GRSMU.Bot.Web.Api.Models;
using GRSMU.Bot.Web.Api.Models.Gradebook;
using GRSMU.Bot.Web.Core.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GRSMU.Bot.Web.Api.Controllers
{
    [ApiController]
    [Route("api/gradebook")]
    [Authorize]
    public class GradebookController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IResourceProvider _resourceProvider;
        private readonly IMapper _mapper;

        public GradebookController(
            ISender sender,
            IResourceProvider resourceProvider,
            IMapper mapper)
        {
            _sender = sender;
            _resourceProvider = resourceProvider;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GradebookModel>> GetGradebookAsync(string disciplineId, bool force)
        {
            var userId = HttpContext.User.GetId();

            var gradebookResult = await _sender.Send(new GetDisciplineGradebookQuery(userId, disciplineId, force));

            if (gradebookResult.HasErrors)
            {
                return gradebookResult.ToFailureActionResult(_resourceProvider);
            }

            var model = _mapper.Map<GradebookModel>(gradebookResult.Data);

            return Ok(model);
        }

        [HttpGet("discipline/lookup")]
        public async Task<ActionResult<List<LookupModel>>> GetDisciplineLookupAsync(string? searchQuery)
        {
            var userId = HttpContext.User.GetId();

            var lookupResult = await _sender.Send(new GetDisciplineLookupQuery(userId, searchQuery));

            if (lookupResult.HasErrors)
            {
                return lookupResult.ToFailureActionResult(_resourceProvider);
            }

            var models = _mapper.Map<List<LookupModel>>(lookupResult.Data);

            return Ok(models);
        }
    }
}
