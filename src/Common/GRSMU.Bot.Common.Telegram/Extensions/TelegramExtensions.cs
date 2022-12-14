using GRSMU.Bot.Common.Contexts;
using GRSMU.Bot.Common.Telegram.Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace GRSMU.Bot.Common.Telegram.Extensions;

public static class TelegramExtensions
{
    public static Chat GetChat(this Update update) => update.Type switch
    {
        UpdateType.CallbackQuery => update.CallbackQuery.Message.Chat,
        UpdateType.Message => update.Message.Chat,
        _ => throw new NotSupportedException()
    };

    public static long GetChatId(this Update update) => update.Type switch
    {
        UpdateType.CallbackQuery => update.CallbackQuery.Message.Chat.Id,
        UpdateType.Message => update.Message.Chat.Id,
        _ => throw new NotSupportedException()
    };

    public static string GetMessageText(this Update update) => update.Type switch
    {
        UpdateType.Message => update?.Message?.Text ?? String.Empty,
        _ => string.Empty
    };

    public static User GetUser(this Update update) => update.Type switch
    {
        UpdateType.CallbackQuery => update.CallbackQuery.From,
        UpdateType.Message => update.Message.From,
        _ => throw new NotSupportedException()
    };

    public static string ExtractCommand(this Update update) => update.Type switch
    {
        UpdateType.CallbackQuery => CallbackDataProcessor.ReadCallbackData(update?.CallbackQuery?.Data).Handler ??
                                    String.Empty,
        UpdateType.Message => update.GetMessageText(),
        _ => throw new NotSupportedException()
    };

    public static Task<Message> SendTextMessage(this ITelegramBotClient client, UserContext userContext, string message)
    {
        return client.SendTextMessageAsync(userContext.ChatId, message);
    }

    public static Task<Message> SendTextMessageWithMarkup(this ITelegramBotClient client, UserContext userContext,
        string message, IReplyMarkup markup)
    {
        return client.SendTextMessageAsync(userContext.ChatId, replyMarkup: markup, text: message);
    }

    public static async Task RemoveReplyKeyboard(this ITelegramBotClient client, UserContext userContext)
    {
        var serviceMessage = await client.SendTextMessageWithMarkup
        (
            userContext,
            $"Service message: {Guid.NewGuid()}",
            new ReplyKeyboardRemove()
        );

        await client.DeleteMessageAsync
        (
            userContext.ChatId,
            serviceMessage.MessageId
        );
    }

    public static bool IsTextMessage(this Update update)
    {
        return !string.IsNullOrWhiteSpace(GetMessageText(update));
    }

    public static async Task SetReplyKeyboard(this ITelegramBotClient client, UserContext userContext, ReplyKeyboardMarkup replyMarkup)
    {
        var serviceMessage = await client.SendTextMessageWithMarkup
        (
            userContext,
            $"Service message: {Guid.NewGuid()}",
            replyMarkup
        );

        await client.DeleteMessageAsync
        (
            userContext.ChatId,
            serviceMessage.MessageId
        );
    }
}