using AutoMapper;
using GRSMU.Bot.Common.Contexts;
using GRSMU.Bot.Common.Services;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Data.Users.Documents;
using Telegram.Bot.Types;

namespace GRSMU.Bot.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<UserContext> CreateContextFromTelegramUpdateAsync(Update update)
        {
            var user = update.GetUser();

            var telegramId = user.Id.ToString();

            var userDocument = await _userRepository.GetByTelegramIdAsync(telegramId);

            if (userDocument == null)
            {
                userDocument = _mapper.Map<UserDocument>(user);

                userDocument.ChatId = update.GetChatId();

                await _userRepository.InsertAsync(userDocument);
            }

            var context = _mapper.Map<UserContext>(userDocument);

            return context;
        }

        public Task UpdateContext(UserContext context)
        {
            var document = _mapper.Map<UserDocument>(context);

            return _userRepository.UpdateOneAsync(document);
        }

        public Task UpdateLastMessageBotId(UserContext context, int messageId)
        {
            context.LastBotMessageId = messageId;
            
            return UpdateContext(context);
        }

        public Task DeleteLastMessageBotId(UserContext context)
        {
            context.LastBotMessageId = null;

            return UpdateContext(context);
        }
    }
}
