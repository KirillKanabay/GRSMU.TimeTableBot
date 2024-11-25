using GRSMU.Bot.Common.Messaging;

namespace GRSMU.Bot.Logic.Features.Users.Commands.UpdateRefreshToken
{
    public record UpdateRefreshTokenCommand(
        string UserId,
        string Token,
        DateTime ExpireTime) : ICommand;
}
