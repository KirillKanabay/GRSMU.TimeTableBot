using AngleSharp.Io;
using AutoMapper;
using GRSMU.Bot.Common.Resources;
using GRSMU.Bot.Logic.Features.Users.Commands.UpdateStudentCardId;
using GRSMU.Bot.Logic.Features.Users.Commands.UpdateUserFaculty;
using GRSMU.Bot.Logic.Features.Users.Queries.GetById;
using GRSMU.Bot.Web.Api.Extensions;
using GRSMU.Bot.Web.Api.Models.User.Requests;
using GRSMU.Bot.Web.Api.Models.User.Responses;
using GRSMU.Bot.Web.Core.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GRSMU.Bot.Web.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;
        private readonly IResourceProvider _resourceProvider;

        public UserController(
            ISender sender, 
            IMapper mapper, 
            IResourceProvider resourceProvider)
        {
            _sender = sender;
            _mapper = mapper;
            _resourceProvider = resourceProvider;
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserMeResponse>> GetCurrentUserAsync()
        {
            var id = HttpContext.User.GetId();

            var searchResult = await _sender.Send(new GetUserByIdQuery(id));

            if (searchResult.HasErrors)
            {
                return searchResult.ToFailureActionResult(_resourceProvider);
            }

            var responseModel = _mapper.Map<UserMeResponse>(searchResult.Data);

            return Ok(responseModel);
        }

        [HttpPut("student-card-id")]
        public async Task<ActionResult<UserPrefilledFacultyModel>> UpdateStudentCardIdAsync(UpdateStudentCardIdRequest request)
        {
            var id = HttpContext.User.GetId();

            var command = new UpdateStudentCardIdCommand(id, request.Login, request.Password, request.FacultyId);

            var result = await _sender.Send(command);

            if (result.HasErrors)
            {
                return result.ToFailureActionResult(_resourceProvider);
            }

            var model = _mapper.Map<UserPrefilledFacultyModel>(result.Data);

            return Ok(model);
        }

        [HttpPut("student-faculty")]
        public async Task<ActionResult> UpdateStudentFacultyAsync(UpdateStudentFacultyRequest request)
        {
            var id = HttpContext.User.GetId();

            var command = new UpdateUserFacultyCommand(id, request.FacultyId, request.CourseId, request.GroupId);
            
            var result = await _sender.Send(command);
            
            return result.HasErrors ? result.ToFailureActionResult(_resourceProvider) : Ok();
        }
    }
}
