using GRSMU.Bot.Common.Models.Responses;
using GRSMU.Bot.Common.Extensions;
using GRSMU.Bot.Common.Models.Responses;
using GRSMU.Bot.Common.Services;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Core.Immutable;
using GRSMU.Bot.Data.Common.Contracts;
using GRSMU.Bot.Domain.Reports.Requests;
using Telegram.Bot;
using GRSMU.Bot.Common.Telegram.Brokers.Handlers;
using GRSMU.Bot.Common.Telegram.Enums;

namespace GRSMU.Bot.Application.Features.Users.TelegramHandlers;

public class CancelReportRequestHandler : TelegramRequestHandlerBase<CancelReportRequestMessage>
{
    private readonly IUserService _userService;
    private readonly IRequestCacheRepository _requestCacheRepository;

    public CancelReportRequestHandler(ITelegramBotClient client, IUserService userService, IRequestCacheRepository requestCacheRepository) : base(client)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _requestCacheRepository = requestCacheRepository ?? throw new ArgumentNullException(nameof(requestCacheRepository));
    }

    protected override async Task<TelegramResponse> ExecuteAsync(CancelReportRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new TelegramResponse(request.UserContext, TelegramResponseStatus.Finished);

        var user = request.UserContext;

        await Client.DeleteMessageAsync(user.ChatId, user.LastBotMessageId.Value, cancellationToken);

        user.LastBotMessageId = null;
        await _userService.UpdateContext(user);

        await _requestCacheRepository.Pop(user.TelegramId);

        await Client.SendTextMessageWithMarkup(user, "Команда отменена", Markups.DefaultMarkup);

        return response;
    }
}