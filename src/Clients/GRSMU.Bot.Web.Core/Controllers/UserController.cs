using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GRSMU.Bot.Common.Broker.Contracts;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Common.Web.ViewModels;
using GRSMU.Bot.Domain.Users.Dtos.Filters;
using GRSMU.Bot.Domain.Users.Requests;
using GRSMU.Bot.Web.Core.ViewModels.Users;

namespace GRSMU.Bot.Web.Core.Controllers;

public class UserController : Controller
{
    private readonly IRequestBroker _requestBroker;
    private readonly IMapper _mapper;

    public UserController(IRequestBroker requestBroker, IMapper mapper)
    {
        _requestBroker = requestBroker ?? throw new ArgumentNullException(nameof(requestBroker));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IActionResult> Index([FromQuery] int? page)
    {
        var response = await _requestBroker.Publish(new GetUsersRequestMessage
        {
            Filter = new UserFilterDto(),
            Paging = new PagingModel
            {
                Page = page ?? 1,
                PageSize = 10
            }
        });

        var viewModel = new UserListViewModel
        {
            Users = response.Items.Select(_mapper.Map<UserViewModel>).ToList(),
            Paging = _mapper.Map<PagingViewModel>(response.PagingModel)
        };

        return View(viewModel);
    }
}