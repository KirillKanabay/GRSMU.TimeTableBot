using AutoMapper;
using GRSMU.TimeTableBot.Common.Broker.Handlers;
using GRSMU.TimeTableBot.Common.Broker.RequestBroker;
using GRSMU.TimeTableBot.Common.Common.Handlers.Data;
using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.Extensions;
using GRSMU.TimeTableBot.Common.Models.Responses;
using GRSMU.TimeTableBot.Common.Services;
using GRSMU.TimeTableBot.Core.DataLoaders;
using GRSMU.TimeTableBot.Core.Immutable;
using GRSMU.TimeTableBot.Data.Users.Contracts;
using GRSMU.TimeTableBot.Data.Users.Documents;
using GRSMU.TimeTableBot.Domain.Users.TelegramRequests.Settings;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace GRSMU.TimeTableBot.Application.Users.TelegramHandlers.Settings
{
    public abstract class SettingsRequestHandlerBase<TRequest> : TelegramRequestHandlerBase<TRequest>
        where TRequest : SettingsRequestMessageBase
    {
        protected abstract string SettingsTitle { get; }
        protected abstract string CallbackHandlerName { get; }
        protected virtual int ItemsInRow => 1;
        
        protected abstract bool IsFirstHandler { get; }
        protected abstract bool IsLastHandler { get; }

        protected readonly IRequestBroker RequestBroker;
        protected readonly FormDataLoader FormDataLoader;
        protected readonly IUserService UserService;

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        protected SettingsRequestHandlerBase(ITelegramBotClient client, IRequestBroker requestBroker, IUserService userService, IMapper mapper, IUserRepository userRepository, FormDataLoader formDataLoader) : base(client)
        {
            RequestBroker = requestBroker ?? throw new ArgumentNullException(nameof(requestBroker));
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            FormDataLoader = formDataLoader ?? throw new ArgumentNullException(nameof(formDataLoader));
        }

        protected override async Task<EmptyResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken)
        {
            var response = new EmptyResponse(request.UserContext);

            var user = request.UserContext;

            if (request.BackExecuted && !IsFirstHandler)
            {
                await ExecuteBackHandler(user);
                return response;
            }

            var settingsItems = await GetSettingItems(user);

            if (string.IsNullOrWhiteSpace(request.Value))
            {
                var buttons = CreateButtons(settingsItems, ItemsInRow);

                if (user.LastBotMessageId.HasValue)
                {
                    await Client.EditMessageTextAsync
                    (
                        user.ChatId,
                        user.LastBotMessageId.Value,
                        SettingsTitle,
                        replyMarkup: buttons,
                        cancellationToken: cancellationToken
                    );
                }
                else
                {
                    var message = await Client.SendTextMessageWithMarkup(user, SettingsTitle, buttons);

                    await UserService.UpdateLastMessageBotId(user, message.MessageId);
                }

                return response;
            }

            if (settingsItems.Values.Contains(request.Value))
            {
                UpdateSettingItem(user, request.Value);

                var userDocument = _mapper.Map<UserDocument>(user);

                await _userRepository.UpdateOneAsync(userDocument);
            }

            if (!IsLastHandler)
            {
                await ExecuteNextHandler(user);
            }

            return response;
        }

        protected abstract void UpdateSettingItem(UserContext user, string value);

        protected abstract Task<IReadOnlyDictionary<string, string>> GetSettingItems(UserContext user);

        protected abstract Task ExecuteNextHandler(UserContext user);

        protected abstract Task ExecuteBackHandler(UserContext user);

        protected virtual InlineKeyboardMarkup CreateButtons(IReadOnlyDictionary<string, string> items, int itemsInRow = 1)
        {
            var buttons = items.Select
            (
                x => InlineKeyboardButton.WithCallbackData
                (
                    text: x.Key,
                    callbackData: CallbackDataProcessor.CreateCallbackData(new CallbackData
                    {
                        Data = x.Value, 
                        Handler = CallbackHandlerName
                    })
                )
            ).Chunk(itemsInRow).ToList();

            if (!IsFirstHandler)
            {
                buttons.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData
                    (
                        text: "Назад",
                        callbackData: CallbackDataProcessor.CreateCallbackData(new CallbackData
                        {
                            Data = CommandKeys.Registrators.Back,
                            Handler = CallbackHandlerName,
                        })
                    )
                });
            }
            
            return new InlineKeyboardMarkup(buttons);
        }
    }
}
