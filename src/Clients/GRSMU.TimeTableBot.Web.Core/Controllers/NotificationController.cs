using GRSMU.TimeTableBot.Web.Core.ViewModels.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace GRSMU.TimeTableBot.Web.Core.Controllers;

public class NotificationController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new NotificationViewModel
        {
            Filter = new NotificationFilterViewModel()
        });
    }

    [HttpPost]
    public async Task<IActionResult> SendNotification(NotificationViewModel viewModel)
    {
        return RedirectToAction("Index");
    }
}