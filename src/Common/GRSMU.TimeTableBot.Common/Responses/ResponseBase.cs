using GRSMU.TimeTableBot.Common.Broker.Responses;
using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Common.Responses
{
    public abstract class ResponseBase
    {
        public IUserContext UserContext { get; set; }

        public ResponseStatus Status { get; set; }

        protected ResponseBase(IUserContext userContext, ResponseStatus status)
        {
            UserContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            Status = status;
        }
    }
}
