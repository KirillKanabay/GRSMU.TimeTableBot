using AutoMapper;
using GRSMU.Bot.Common.Broker.Contexts;
using GRSMU.Bot.Common.Broker.RequestHandlers;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Common.Models.Messages;
using GRSMU.Bot.Common.Models.Responses;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Common.Telegram.Immutable;
using GRSMU.Bot.Common.Telegram.Models;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Data.Users.Contracts.Filters;
using GRSMU.Bot.Domain.Notifications.Dto;
using GRSMU.Bot.Domain.Notifications.Enums;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace GRSMU.Bot.Application.Features.Notifications.Handlers
{
    public class NotifyUsersRequestHandler : CommandHandlerBase<NotifyUsersRequestMessage>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<NotifyUsersRequestHandler> _logger;

        public NotifyUsersRequestHandler(IUserRepository userRepository, IMapper mapper, ITelegramBotClient botClient, ILogger<NotifyUsersRequestHandler> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task<EmptyResponse> ExecuteAsync(NotifyUsersRequestMessage request, NullableContext context)
        {
            var filter = request.Filter;

            var records = await _userRepository.GetUserListAsync(new UserFilter(), new PagingModel());

            var users = records.Select(_mapper.Map<TelegramUser>).Where
            (
                x => filter.Type is NotificationType.Broadcast || 
                     filter.Type is NotificationType.OnlyRegistered && x.IsRegistered()
            ).ToList();

            foreach (var telegramUser in users)
            {
                try
                {
                    switch (filter.Type)
                    {
                        case NotificationType.Broadcast:
                            await _botClient.SendTextMessage(telegramUser, request.Text);
                            break;
                        case NotificationType.OnlyRegistered:
                            await _botClient.SendTextMessageWithMarkup(telegramUser, request.Text, Markups.DefaultMarkup);
                            break;
                    }
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, "Error while notifying user {telegramUser}", telegramUser);
                }
            }

            return new EmptyResponse();
        }
    }

    public class NotifyUsersRequestMessage : CommandMessageBase
    {
        public string Text { get; set; }

        public NotificationFilterDto Filter { get; set; }
    }
}
