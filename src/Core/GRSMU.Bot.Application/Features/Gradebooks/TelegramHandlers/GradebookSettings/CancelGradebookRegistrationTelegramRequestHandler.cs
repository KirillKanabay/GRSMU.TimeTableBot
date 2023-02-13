using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Handlers;
using GRSMU.Bot.Common.Telegram.Brokers.RequestCache;
using GRSMU.Bot.Common.Telegram.Immutable;
using GRSMU.Bot.Common.Telegram.Models.Messages;
using GRSMU.Bot.Common.Telegram.Services;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace GRSMU.Bot.Application.Features.Gradebooks.TelegramHandlers.GradebookSettings;

public class CancelGradebookRegistrationTelegramRequestHandler : TelegramCancelRequestHandlerBase<CancelGradebookRegistrationTelegramRequestMessage>
{
    public CancelGradebookRegistrationTelegramRequestHandler(ITelegramBotClient client, ITelegramRequestContext context, ITelegramUserService userService, IRequestCache requestCache) : base(client, context, userService, requestCache)
    {
    }

    protected override IReplyMarkup Markup => Markups.DefaultMarkup;
}

public class CancelGradebookRegistrationTelegramRequestMessage : TelegramCommandMessageBase
{
}