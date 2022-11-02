using AutoMapper;
using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.Extensions;
using GRSMU.TimeTableBot.Common.Services;
using GRSMU.TimeTableBot.Data.Documents;
using GRSMU.TimeTableBot.Data.Repositories.Users;
using Telegram.Bot.Types;

namespace GRSMU.TimeTableBot.Core.Services
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

            var userDocument = await _userRepository.GetByTelegramId(telegramId);

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
