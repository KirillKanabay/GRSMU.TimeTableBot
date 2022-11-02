using Microsoft.AspNetCore.Mvc;

namespace GRSMU.TimeTableBot.Api.Controllers;

[ApiController]
public class ApplicationController : ControllerBase
{
    [HttpGet("Live")]
    public IActionResult Live()
    {
        return Ok("Server working!");
    }
}