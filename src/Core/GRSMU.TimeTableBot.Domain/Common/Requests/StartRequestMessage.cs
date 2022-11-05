using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.RequestMessages;

namespace GRSMU.TimeTableBot.Domain.Common.Requests
{
    public class StartRequestMessage : TelegramRequestMessageBase
    {
        public StartRequestMessage(UserContext userContext) : base(userContext)
        {
        }
    }
}
