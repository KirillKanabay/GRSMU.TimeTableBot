using GRSMU.TimeTableBot.Common.Contexts;

namespace GRSMU.TimeTableBot.Common.Responses;

public abstract class TelegramResponseBase : ResponseBase
{
    public IUserContext UserContext { get; set; }

    public ResponseStatus Status { get; set; }

    protected TelegramResponseBase(IUserContext userContext, ResponseStatus status)
    {
        UserContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        Status = status;
    }
}