using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.RequestMessages;

namespace GRSMU.TimeTableBot.Domain.RequestMessages.Users.Settings
{
    public class RunSettingsRequestMessage : RequestMessageBase
    {
        public RunSettingsRequestMessage(UserContext userContext) : base(userContext)
        {
        }
    }
}
