using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.RequestMessages;

namespace GRSMU.TimeTableBot.Domain.RequestMessages.Common
{
    public class StartRequestMessage : RequestMessageBase
    {
        public StartRequestMessage(UserContext userContext) : base(userContext)
        {
        }
    }
}
