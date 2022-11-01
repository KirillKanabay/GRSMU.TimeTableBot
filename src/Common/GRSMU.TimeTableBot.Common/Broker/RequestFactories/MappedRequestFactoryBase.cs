using GRSMU.TimeTableBot.Common.Broker.Messages.Factory;
using GRSMU.TimeTableBot.Common.Broker.RequestCache;
using GRSMU.TimeTableBot.Common.Extensions;
using GRSMU.TimeTableBot.Common.RequestMessages;
using GRSMU.TimeTableBot.Common.Services;
using Telegram.Bot.Types;

namespace GRSMU.TimeTableBot.Common.Broker.RequestFactories
{
    public abstract class MappedRequestFactoryBase : IRequestFactory
    {
        private readonly IRequestCache _requestCache;
        private readonly Dictionary<string, Func<Update, bool, Task<RequestMessageBase>>> _requestMap;

        protected readonly IUserService UserService;

        protected MappedRequestFactoryBase(IRequestCache requestCache, IUserService userService)
        {
            _requestCache = requestCache ?? throw new ArgumentNullException(nameof(requestCache));
            _requestMap = new Dictionary<string, Func<Update, bool, Task<RequestMessageBase>>>();
            UserService = userService;
        }
        
        public async Task<RequestMessageBase> CreateRequestMessage(Update update)
        {
            var userContext = await UserService.CreateContextFromTelegramUpdateAsync(update);
            
            var cachedCommand = await _requestCache.Pop(userContext.TelegramId);

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

        public MappedRequestFactoryBase AddRequest(string command, Func<Update, bool, Task<RequestMessageBase>> requestFactory)
        {
            if (_requestMap.ContainsKey(command))
            {
                throw new ArgumentException($"Command {command} already exists");
            }

            _requestMap[command] = requestFactory;

            return this;
        }

        public MappedRequestFactoryBase AddRequest<TRequest>(string command)
            where TRequest : RequestMessageBase
        {
            if (_requestMap.ContainsKey(command))
            {
                throw new ArgumentException($"Command {command} already exists");
            }

            _requestMap[command] = CreateCommandRequestMessage<TRequest>;

            return this;
        }

        protected async Task<RequestMessageBase> CreateCommandRequestMessage<TRequest>(Update update, bool isCached)
            where TRequest : RequestMessageBase
        {
            var userContext = await UserService.CreateContextFromTelegramUpdateAsync(update);
            var requestMessage = Activator.CreateInstance(typeof(TRequest), userContext);

            return (requestMessage as TRequest)!;
        }
    }
}
