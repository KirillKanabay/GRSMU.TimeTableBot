using AutoMapper;
using GRSMU.Bot.Application.Features.Timetables.DataLoaders;
using GRSMU.Bot.Common.Telegram.Data;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Core.Immutable;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Data.Users.Documents;
using GRSMU.Bot.Domain.Users.TelegramRequests.Settings;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Contracts;
using GRSMU.Bot.Common.Telegram.Brokers.Handlers;
using GRSMU.Bot.Common.Telegram.Models;
using GRSMU.Bot.Common.Telegram.Services;

namespace GRSMU.Bot.Application.Features.Users.TelegramHandlers.Settings
{
    public abstract class SettingsRequestHandlerBase<TRequest> : SimpleTelegramRequestHandlerBase<TRequest>
        where TRequest : SettingsRequestMessageBase
    {
        protected abstract string SettingsTitle { get; }
        protected abstract string CallbackHandlerName { get; }
        protected virtual int ItemsInRow => 1;

        protected abstract bool IsFirstHandler { get; }
        protected abstract bool IsLastHandler { get; }

        protected readonly ITelegramRequestBroker RequestBroker;
        protected readonly FormDataLoader FormDataLoader;
        protected readonly ITelegramUserService UserService;

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        protected SettingsRequestHandlerBase(
            ITelegramBotClient client,
            ITelegramRequestBroker requestBroker, 
            ITelegramUserService userService, 
            IMapper mapper, 
            IUserRepository userRepository, 
            FormDataLoader formDataLoader,
            ITelegramRequestContext context) : base(client, context)
        {
            RequestBroker = requestBroker ?? throw new ArgumentNullException(nameof(requestBroker));
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            FormDataLoader = formDataLoader ?? throw new ArgumentNullException(nameof(formDataLoader));
        }

        protected override async Task ExecuteAsync(TRequest request, CancellationToken cancellationToken)
        {
            var user = Context.User;

            if (request.BackExecuted && !IsFirstHandler)
            {
                await ExecuteBackHandler(user);
                return;
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

                    await UserService.UpdateLastMessageBotIdAsync(user, message.MessageId);
                }

                return;
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

            return;
        }

        protected abstract void UpdateSettingItem(TelegramUser user, string value);

        protected abstract Task<IReadOnlyDictionary<string, string>> GetSettingItems(TelegramUser user);

        protected abstract Task ExecuteNextHandler(TelegramUser user);

        protected abstract Task ExecuteBackHandler(TelegramUser user);

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
