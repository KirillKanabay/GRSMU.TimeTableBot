using GRSMU.TimeTableBot.Common.Broker.TelegramBroker;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace GRSMU.TimeTableBot.Web.Core.Controllers;

public class WebhookController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromServices] ITelegramRequestBroker requestBroker, [FromBody] Update update)
    {
        await requestBroker.HandleUpdateAsync(update);

        return Ok();
    }
}