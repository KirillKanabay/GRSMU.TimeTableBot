using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Web.Api.Models;

namespace GRSMU.Bot.Web.Api.Services.Interfaces
{
    public interface ITelegramTokenValidator
    {
        ExecutionResult<TelegramAppUser> Validate(string token);
    }
}
