using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Data;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Data.Reports.Contracts;
using GRSMU.Bot.Data.Reports.Documents;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using GRSMU.Bot.Common.Telegram.Brokers.Handlers;
using GRSMU.Bot.Common.Telegram.Enums;
using GRSMU.Bot.Common.Telegram.Immutable;
using GRSMU.Bot.Common.Telegram.Models.Responses;
using GRSMU.Bot.Common.Telegram.Services;
using GRSMU.Bot.Domain.Reports.TelegramRequests;
using static System.String;

namespace GRSMU.Bot.Application.Features.Users.TelegramHandlers;

public class ReportRequestHandler : TelegramRequestHandlerBase<ReportRequestMessage>
{
    private readonly ITelegramUserService _userService;
    private readonly IReportRepository _reportRepository;
    
    public ReportRequestHandler(ITelegramBotClient client, ITelegramUserService userService, IReportRepository reportRepository, ITelegramRequestContext context) : base(client, context)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
    }

    protected override async Task<TelegramResponse> ExecuteAsync(ReportRequestMessage request, CancellationToken cancellationToken)
    {
        var user = Context.User;

        if (IsNullOrEmpty(request.Message))
        {
            await Client.RemoveReplyKeyboard(user);

            var message = await Client.SendTextMessageWithMarkup
            (
                user,
                "Пожалуйста, опиши свою проблему",
                new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData
                (
                    text: "Отмена",
                    callbackData: CallbackDataProcessor.CreateCallbackData(new CallbackData
                    {
                        Handler = CommandKeys.Reports.Cancel,
                        Data = "_"
                    })
                ))
            );

            user.LastBotMessageId = message.MessageId;

            await _userService.UpdateUserAsync(user);

            return new TelegramResponse(TelegramResponseStatus.WaitingNextResponse, CommandKeys.Reports.Report);
        }

        await Client.EditMessageReplyMarkupAsync
        (
            user.ChatId,
            user.LastBotMessageId.Value,
            InlineKeyboardMarkup.Empty(),
            cancellationToken
        );

        user.LastBotMessageId = null;

        await _userService.UpdateUserAsync(user);

        await _reportRepository.InsertAsync(new ReportDocument
        {
            Message = request.Message,
            TelegramId = user.TelegramId
        });

        await Client.SendTextMessageWithMarkup
        (
            user,
            "Жалоба принята. Администратор свяжется с тобой в ближайшее время.",
            Markups.DefaultMarkup
        );

        return new TelegramResponse();
    }
}