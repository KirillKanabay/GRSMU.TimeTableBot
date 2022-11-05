using Microsoft.AspNetCore.Mvc;

namespace GRSMU.TimeTableBot.Web.Core.Controllers;

[ApiController]
public class ApplicationController : ControllerBase
{
    [HttpGet("Live")]
    public IActionResult Live()
    {
        return Ok("Server working!");
    }
}