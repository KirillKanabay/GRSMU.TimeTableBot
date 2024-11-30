using GRSMU.Bot.Common.Resources;
using GRSMU.Bot.Logic.Features.Users.Commands.GetOrCreateUser;
using GRSMU.Bot.Web.Api.Extensions;
using GRSMU.Bot.Web.Api.Models.Account.Requests;
using GRSMU.Bot.Web.Core.Authorization.Models;
using GRSMU.Bot.Web.Core.Authorization.Services.Interfaces;
using GRSMU.Bot.Web.Core.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GRSMU.Bot.Web.Api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITelegramTokenValidator _telegramTokenValidator;
        private readonly ISender _sender;
        private readonly IResourceProvider _resourceProvider;

        public AccountController(
            ITelegramTokenValidator telegramTokenValidator,
            ISender sender, 
            IAccountService accountService,
            IResourceProvider resourceProvider)
        {
            _telegramTokenValidator = telegramTokenValidator;
            _sender = sender;
            _accountService = accountService;
            _resourceProvider = resourceProvider;
        }

        [HttpPost]
        [Route("tg-auth")]
        public async Task<ActionResult<TokenModel>> Authorization(AuthorizationRequest request)
        {
            var validationResult = _telegramTokenValidator.Validate(request.Token);

            if (!validationResult.IsSuccess)
            {
                return validationResult.ToFailureActionResult(_resourceProvider);
            }

            var telegramUser = validationResult.Data;

            var getOrCreateResult = await _sender.Send(new GetOrCreateUserCommand
            {
                ChatId = request.ChatId,
                TelegramId = telegramUser.Id.ToString(),
                TelegramFirstName = telegramUser.FirstName,
                TelegramLastName = telegramUser.LastName,
                TelegramUsername = telegramUser.UserName
            });

            if (getOrCreateResult.HasErrors)
            {
                return getOrCreateResult.ToFailureActionResult(_resourceProvider);
            }

            var token = await _accountService.LoginAsync(getOrCreateResult.Data);

            return Ok(token);
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<ActionResult<TokenModel>> RefreshToken(UpdateRefreshTokenRequest request)
        {
            var updateTokenResult = await _accountService.RefreshAsync(request.AccessToken, request.RefreshToken);

            if (updateTokenResult.HasErrors)
            {
                return updateTokenResult.ToFailureActionResult(_resourceProvider);
            }

            return Ok(updateTokenResult.Data);
        }
    }
}
