using Microsoft.AspNetCore.Mvc;

namespace GRSMU.TimeTableBot.Web.Core.Controllers;

public class UserController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}