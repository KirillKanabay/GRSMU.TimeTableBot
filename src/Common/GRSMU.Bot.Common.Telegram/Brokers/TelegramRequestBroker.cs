using GRSMU.Bot.Common.Telegram.Brokers.Contracts;
using GRSMU.Bot.Common.Telegram.Models.Messages;
using GRSMU.Bot.Common.Telegram.Models.Responses;
using MediatR;

namespace GRSMU.Bot.Common.Telegram.Brokers;

public class TelegramRequestBroker : ITelegramRequestBroker
{
    private readonly IMediator _mediator;

    public TelegramRequestBroker(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<TelegramResponse> Publish(TelegramCommandMessageBase request)
    {
        var response = await _mediator.Send(request);

        return response;
    }
}