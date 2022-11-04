using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.RequestMessages;

namespace GRSMU.TimeTableBot.Domain.RequestMessages.Common
{
    public class StartRequestMessage : TelegramRequestMessageBase
    {
        public StartRequestMessage(UserContext userContext) : base(userContext)
        {
        }
    }
}
