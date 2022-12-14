using GRSMU.Bot.Common.Telegram.Brokers;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace GRSMU.Bot.Web.Core.Controllers;

public class WebhookController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromServices] ITelegramRequestBroker requestBroker, [FromBody] Update update)
    {
        await requestBroker.HandleUpdateAsync(update);

        return Ok();
    }
}