using GRSMU.Bot.Common.Contexts;

namespace GRSMU.Bot.Domain.Common.Requests
{
    public class StartRequestMessage : TelegramRequestMessageBase
    {
        public StartRequestMessage(UserContext userContext) : base(userContext)
        {
        }
    }
}
