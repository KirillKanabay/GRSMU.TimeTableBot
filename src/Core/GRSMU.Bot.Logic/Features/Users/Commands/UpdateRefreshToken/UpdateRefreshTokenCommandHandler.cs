using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Data.Users.Contracts;

namespace GRSMU.Bot.Logic.Features.Users.Commands.UpdateRefreshToken
{
    public class UpdateRefreshTokenCommandHandler : ICommandHandler<UpdateRefreshTokenCommand>
    {
        private readonly IUserRepository _userRepository;

        public UpdateRefreshTokenCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ExecutionResult> Handle(UpdateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            await _userRepository.UpdateRefreshToken(
                request.UserId,
                request.Token,
                request.ExpireTime);

            return ExecutionResult.Success();
        }
    }
}
