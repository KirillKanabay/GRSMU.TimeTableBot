﻿using AutoMapper;
using GRSMU.Bot.Logic.Features.Users.Commands.UpdateStudentCardId;
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

        public UserController(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserMeResponse>> GetCurrentUserAsync()
        {
            var id = HttpContext.User.GetId();

            var searchResult = await _sender.Send(new GetUserByIdQuery(id));

            if (searchResult.HasErrors)
            {
                return searchResult.ToFailureActionResult();
            }

            var responseModel = _mapper.Map<UserMeResponse>(searchResult.Data);

            return Ok(responseModel);
        }

        [HttpPut("student-card-id")]
        public async Task<ActionResult<UserPrefilledFacultyModel>> UpdateStudentCardId(UpdateStudentCardIdRequest request)
        {
            var id = HttpContext.User.GetId();

            var command = new UpdateStudentCardIdCommand(id, request.Login, request.Password, request.FacultyId);

            var result = await _sender.Send(command);

            if (result.HasErrors)
            {
                return result.ToFailureActionResult();
            }

            var model = _mapper.Map<UserPrefilledFacultyModel>(result.Data);

            return Ok(model);
        }
    }
}
