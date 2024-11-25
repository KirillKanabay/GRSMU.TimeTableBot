using AutoMapper;
using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Data.Users.Documents;
using GRSMU.Bot.Logic.Features.Users.Dtos;

namespace GRSMU.Bot.Logic.Features.Users.Commands.GetOrCreateUser
{
    internal class GetOrCreateUserCommandHandler : ICommandHandler<GetOrCreateUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetOrCreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ExecutionResult<UserDto>> Handle(GetOrCreateUserCommand request, CancellationToken cancellationToken)
        {
            var userDocument = await _userRepository.GetByTelegramIdAsync(request.TelegramId);

            if (userDocument is null)
            {
                userDocument = CreateDocumentByRequest(request);
                await _userRepository.InsertAsync(userDocument);
            }

            var dto = _mapper.Map<UserDto>(userDocument);
            return ExecutionResult.Success(dto);
        }

        private UserDocument CreateDocumentByRequest(GetOrCreateUserCommand request)
        {
            return new UserDocument
            {
                TelegramId = request.TelegramId,
                TelegramFirstName = request.TelegramFirstName,
                TelegramLastName = request.TelegramLastName,
                TelegramUsername = request.TelegramUsername,
                ChatId = request.ChatId
            };
        }
    }
}
