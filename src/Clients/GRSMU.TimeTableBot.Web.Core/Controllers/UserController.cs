using GRSMU.TimeTableBot.Common.Broker.RequestBroker;
using Microsoft.AspNetCore.Mvc;

namespace GRSMU.TimeTableBot.Web.Core.Controllers;

public class UserController : Controller
{
    private readonly IRequestBroker _requestBroker;

    public UserController(IRequestBroker requestBroker)
    {
        _requestBroker = requestBroker ?? throw new ArgumentNullException(nameof(requestBroker));
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }
}