using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Web.Core.Models;

namespace GRSMU.Bot.Web.Core.Services.Interfaces
{
    public interface ITelegramTokenValidator
    {
        ExecutionResult<TelegramAppUser> Validate(string token);
    }
}
