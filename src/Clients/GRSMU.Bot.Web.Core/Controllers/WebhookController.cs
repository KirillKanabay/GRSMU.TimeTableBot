using GRSMU.Bot.Common.Telegram.Brokers.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace GRSMU.Bot.Web.Core.Controllers;

[AllowAnonymous]
public class WebhookController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromServices] ITelegramUpdateHandler requestBroker, [FromBody] Update update)
    {
        await requestBroker.HandleUpdateAsync(update);

        return Ok();
    }
}