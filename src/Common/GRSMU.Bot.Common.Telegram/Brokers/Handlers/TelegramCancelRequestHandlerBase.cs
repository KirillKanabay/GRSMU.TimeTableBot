using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.RequestCache;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Common.Telegram.Models.Messages;
using GRSMU.Bot.Common.Telegram.Services;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace GRSMU.Bot.Common.Telegram.Brokers.Handlers;

public abstract class TelegramCancelRequestHandlerBase<TRequest> : SimpleTelegramRequestHandlerBase<TRequest>
    where TRequest : TelegramCommandMessageBase
{
    protected abstract IReplyMarkup Markup { get; }
    private readonly ITelegramUserService _userService;
    private readonly IRequestCache _requestCache;

    protected TelegramCancelRequestHandlerBase(ITelegramBotClient client, ITelegramRequestContext context, ITelegramUserService userService, IRequestCache requestCache) : base(client, context)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _requestCache = requestCache ?? throw new ArgumentNullException(nameof(requestCache));
    }

    protected override async Task ExecuteAsync(TRequest request, CancellationToken cancellationToken)
    {
        var user = Context.User;

        await Client.DeleteMessageAsync(user.ChatId, user.LastBotMessageId.Value, cancellationToken);

        user.LastBotMessageId = null;
        await _userService.UpdateUserAsync(user);

        await _requestCache.Pop(user.TelegramId);

        await Client.SendTextMessageWithMarkup(user, "Команда отменена", Markup);
    }
}