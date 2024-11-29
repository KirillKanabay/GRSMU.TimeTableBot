using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GRSMU.Bot.Web.Api.Controllers;

[ApiController]
[Route("api/group-info")]
[Authorize]
public class FacultyController : ControllerBase
{
}