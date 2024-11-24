using GRSMU.Bot.Web.Api.Models.Account.Requests;
using GRSMU.Bot.Web.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GRSMU.Bot.Web.Api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ITelegramTokenValidator _telegramTokenValidator;

        public AccountController(ITelegramTokenValidator telegramTokenValidator)
        {
            _telegramTokenValidator = telegramTokenValidator;
        }

        [HttpPost]
        public ActionResult Authorization(AuthorizationRequest request)
        {
            var validationResult = _telegramTokenValidator.Validate(request.Token);

            if (validationResult.IsSuccess)
            {
                return Ok(validationResult.Data);
            }

            return BadRequest();
        }
    }
}
