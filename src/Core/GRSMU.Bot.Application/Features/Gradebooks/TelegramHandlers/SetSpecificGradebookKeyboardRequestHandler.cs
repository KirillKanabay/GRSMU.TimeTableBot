using GRSMU.Bot.Application.Features.Gradebooks.Helpers;
using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Handlers;
using GRSMU.Bot.Common.Telegram.Data;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Common.Telegram.Immutable;
using GRSMU.Bot.Common.Telegram.Models.Messages;
using GRSMU.Bot.Common.Telegram.Services;
using GRSMU.Bot.Domain.Gradebooks.Dtos;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace GRSMU.Bot.Application.Features.Gradebooks.TelegramHandlers;

public class SetSpecificGradebookKeyboardRequestHandler : SimpleTelegramRequestHandlerBase<SetSpecificGradebookKeyboardRequestMessage>
{
    private readonly GradebookProcessor _processor;
    private readonly ITelegramUserService _userService;
    private readonly ILogger<SetSpecificGradebookKeyboardRequestHandler> _logger;

    public SetSpecificGradebookKeyboardRequestHandler(ITelegramBotClient client, ITelegramRequestContext context, GradebookProcessor processor, ITelegramUserService userService, ILogger<SetSpecificGradebookKeyboardRequestHandler> logger) : base(client, context)
    {
        _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(SetSpecificGradebookKeyboardRequestMessage request, CancellationToken cancellationToken)
    {
        var user = Context.User;

        var gradebook = await _processor.GetGradebookDto(user);

        var lastMessageId = user.LastBotMessageId;

        const string message = "Выбери предмет:";

        var markup = CreateMarkup(gradebook);

        if (lastMessageId.HasValue && request.CallbackExecuted && request.LastMessageId.Equals(lastMessageId.ToString()))
        {
            await Client.EditMessageTextAsync
            (
                user.ChatId,
                lastMessageId.Value,
                text: message,
                replyMarkup: markup,
                cancellationToken: cancellationToken
            );
        }
        else
        {
            if (lastMessageId.HasValue)
            {
                try
                {
                    await Client.DeleteMessageAsync(user.ChatId, lastMessageId.Value, cancellationToken: cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, "Error while deleting message for specific gradebook keyboard");
                }
            }

            var sentMessage = await Client.SendTextMessageWithMarkup(user, message, markup);

            user.LastBotMessageId = sentMessage.MessageId;

            await _userService.UpdateUserAsync(user);
        }

    }

    private InlineKeyboardMarkup CreateMarkup(GradebookDto gradebook)
    {
        var disciplines = gradebook.Disciplines.Where(x => x.Marks?.Any() ?? false).ToList();

        var markup = disciplines.Select
        (
            x => InlineKeyboardButton.WithCallbackData
            (
                text: x.Name,
                callbackData: CallbackDataProcessor.CreateCallbackData(new CallbackData
                {
                    Data = x.Id,
                    Handler = CommandKeys.Gradebook.SpecificGradebook
                })
            )
        ).Chunk(1).ToList();

        return new InlineKeyboardMarkup(markup);
    }

}

public class SetSpecificGradebookKeyboardRequestMessage : TelegramCommandMessageBase
{
    public bool CallbackExecuted { get; set; }
    public string LastMessageId { get; set; }
}