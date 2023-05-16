using Microsoft.AspNetCore.Mvc;

namespace GRSMU.Bot.Web.Core.Controllers
{
    [Controller]
    public class AuthenticationController : Controller
    {
        [HttpGet("login")]
        public IActionResult GetLoginPage()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            return Ok();
        }
    }
}
