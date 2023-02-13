using AutoMapper;
using GRSMU.Bot.Application.Features.Notifications.Handlers;
using GRSMU.Bot.Common.Broker.Contracts;
using GRSMU.Bot.Web.Core.ViewModels.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace GRSMU.Bot.Web.Core.Controllers;

public class NotificationController : Controller
{
    private readonly IMapper _mapper;
    private readonly IRequestBroker _requestBroker;

    public NotificationController(IMapper mapper, IRequestBroker requestBroker)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _requestBroker = requestBroker ?? throw new ArgumentNullException(nameof(requestBroker));
    }


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
        var request = _mapper.Map<NotifyUsersRequestMessage>(viewModel);

        await _requestBroker.Publish(request);

        return RedirectToAction("Index");
    }
}