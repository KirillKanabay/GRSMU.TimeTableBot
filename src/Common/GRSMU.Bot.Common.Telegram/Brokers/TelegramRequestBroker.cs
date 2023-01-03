using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Contracts;
using GRSMU.Bot.Common.Telegram.Brokers.RequestCache;
using GRSMU.Bot.Common.Telegram.Enums;
using GRSMU.Bot.Common.Telegram.Models.Messages;
using GRSMU.Bot.Common.Telegram.Models.Responses;
using MediatR;

namespace GRSMU.Bot.Common.Telegram.Brokers;

public class TelegramRequestBroker : ITelegramRequestBroker
{
    private readonly IMediator _mediator;
    private readonly IRequestCache _requestCache;
    private readonly ITelegramRequestContext _requestContext;

    public TelegramRequestBroker(IMediator mediator, IRequestCache requestCache, ITelegramRequestContext requestContext)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _requestCache = requestCache ?? throw new ArgumentNullException(nameof(requestCache));
        _requestContext = requestContext ?? throw new ArgumentNullException(nameof(requestContext));
    }

    public async Task<TelegramResponse> Publish(TelegramCommandMessageBase request)
    {
        var response = await _mediator.Send(request);

        if (response.Status is TelegramResponseStatus.WaitingNextResponse)
        {
            await _requestCache.Push(_requestContext.User.TelegramId, response.Command);
        }

        return response;
    }
}