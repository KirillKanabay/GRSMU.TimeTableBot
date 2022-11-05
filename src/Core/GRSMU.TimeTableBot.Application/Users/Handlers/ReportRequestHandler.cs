using GRSMU.TimeTableBot.Common.Broker.Handlers;
using GRSMU.TimeTableBot.Common.Common.Handlers.Data;
using GRSMU.TimeTableBot.Common.Extensions;
using GRSMU.TimeTableBot.Common.Models.Responses;
using GRSMU.TimeTableBot.Common.Services;
using GRSMU.TimeTableBot.Core.Immutable;
using GRSMU.TimeTableBot.Data.Documents;
using GRSMU.TimeTableBot.Data.Reports.Contracts;
using GRSMU.TimeTableBot.Domain.Reports.Requests;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using static System.String;

namespace GRSMU.TimeTableBot.Application.Users.Handlers;

public class ReportRequestHandler : TelegramRequestHandlerBase<ReportRequestMessage>
{
    private readonly IUserService _userService;
    private readonly IReportRepository _reportRepository;

    public ReportRequestHandler(ITelegramBotClient client, IUserService userService, IReportRepository reportRepository) : base(client)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
    }

    protected override async Task<EmptyResponse> ExecuteAsync(ReportRequestMessage request, CancellationToken cancellationToken)
    {
        var user = request.UserContext;

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

            await _userService.UpdateContext(user);

            return new EmptyResponse(user, ResponseStatus.WaitingNextResponse);
        }

        await Client.EditMessageReplyMarkupAsync
        (
            user.ChatId,
            user.LastBotMessageId.Value,
            InlineKeyboardMarkup.Empty(),
            cancellationToken
        );

        user.LastBotMessageId = null;

        await _userService.UpdateContext(user);

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

        return new EmptyResponse(user, ResponseStatus.Finished);
    }
}