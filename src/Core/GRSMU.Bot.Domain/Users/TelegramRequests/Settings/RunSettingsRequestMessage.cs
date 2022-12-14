using GRSMU.Bot.Common.Models.RequestMessages;
using GRSMU.Bot.Common.Contexts;

namespace GRSMU.Bot.Domain.Users.TelegramRequests.Settings
{
    public class RunSettingsRequestMessage : TelegramRequestMessageBase
    {
        public RunSettingsRequestMessage(UserContext userContext) : base(userContext)
        {
        }
    }
}
