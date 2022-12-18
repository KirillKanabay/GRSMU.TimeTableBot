using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.RequestCache;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Common.Telegram.Models.Messages;
using GRSMU.Bot.Common.Telegram.Services;
using Telegram.Bot.Types;

namespace GRSMU.Bot.Common.Telegram.RequestFactories
{
    public abstract class MappedRequestFactoryBase : IRequestFactory
    {
        private readonly IRequestCache _requestCache;
        private readonly Dictionary<string, Func<Update, bool, Task<TelegramCommandMessageBase>>> _requestMap;

        protected readonly ITelegramUserService UserService;
        protected readonly ITelegramRequestContext Context;

        protected MappedRequestFactoryBase(IRequestCache requestCache, ITelegramUserService userService, ITelegramRequestContext context)
        {
            _requestCache = requestCache ?? throw new ArgumentNullException(nameof(requestCache));
            _requestMap = new Dictionary<string, Func<Update, bool, Task<TelegramCommandMessageBase>>>();
            UserService = userService;
            Context = context;
        }
        
        public async Task<TelegramCommandMessageBase> CreateRequestMessage(Update update)
        {
            var user = Context.User;
            
            var cachedCommand = await _requestCache.Pop(user.TelegramId);

            var command = update.IsTextMessage() 
                ? cachedCommand ?? update.ExtractCommand()
                : update.ExtractCommand();

            if (command == null)
            {
                throw new InvalidOperationException("Could not extract command");
            }

            if (_requestMap.ContainsKey(command))
            {
                return await _requestMap[command].Invoke(update, !string.IsNullOrWhiteSpace(cachedCommand));
            }

#pragma warning disable CS8603
            return null;
#pragma warning restore CS8603
        }

        public MappedRequestFactoryBase AddRequest(string command, Func<Update, bool, Task<TelegramCommandMessageBase>> requestFactory)
        {
            if (_requestMap.ContainsKey(command))
            {
                throw new ArgumentException($"Command {command} already exists");
            }

            _requestMap[command] = requestFactory;

            return this;
        }

        public MappedRequestFactoryBase AddRequest<TRequest>(string command)
            where TRequest : TelegramCommandMessageBase
        {
            if (_requestMap.ContainsKey(command))
            {
                throw new ArgumentException($"Command {command} already exists");
            }

            _requestMap[command] = CreateCommandRequestMessage<TRequest>;

            return this;
        }

        protected async Task<TelegramCommandMessageBase> CreateCommandRequestMessage<TRequest>(Update update, bool isCached)
            where TRequest : TelegramCommandMessageBase
        {
            var TelegramUser = await UserService.CreateUserFromTelegramUpdateAsync(update);
            var requestMessage = Activator.CreateInstance(typeof(TRequest));

            return (requestMessage as TRequest)!;
        }
    }
}
