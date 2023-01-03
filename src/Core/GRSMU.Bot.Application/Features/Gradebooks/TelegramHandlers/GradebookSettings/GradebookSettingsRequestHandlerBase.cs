using AutoMapper;
using GRSMU.Bot.Application.Features.Gradebooks.Helpers;
using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Contracts;
using GRSMU.Bot.Common.Telegram.Brokers.Handlers;
using GRSMU.Bot.Common.Telegram.Data;
using GRSMU.Bot.Common.Telegram.Enums;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Common.Telegram.Immutable;
using GRSMU.Bot.Common.Telegram.Models.Responses;
using GRSMU.Bot.Common.Telegram.Services;
using GRSMU.Bot.Domain.Gradebooks.TelegramRequests.Settings;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace GRSMU.Bot.Application.Features.Gradebooks.TelegramHandlers.GradebookSettings;

public abstract class GradebookSettingsRequestHandlerBase<TRequest> : TelegramRequestHandlerBase<TRequest>
    where TRequest : GradebookSettingsTelegramRequestMessageBase
{
    protected abstract string SettingsTitle { get; }
    protected abstract bool IsFirstHandler { get; }
    protected abstract bool IsLastHandler { get; }
    protected abstract string CallbackHandlerName { get; }

    protected ITelegramRequestBroker RequestBroker { get; }
    protected GradebookProcessor GradebookProcessor { get; }
    protected ITelegramUserService UserService { get; }
    protected IMapper Mapper { get; }
    protected GradebookSettingsRequestHandlerBase(
        ITelegramBotClient client, 
        ITelegramRequestContext context, 
        ITelegramRequestBroker requestBroker, 
        ITelegramUserService userService, 
        IMapper mapper, 
        GradebookProcessor gradebookProcessor) : base(client, context)
    {
        RequestBroker = requestBroker ?? throw new ArgumentNullException(nameof(requestBroker));
        UserService = userService ?? throw new ArgumentNullException(nameof(userService));
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        GradebookProcessor = gradebookProcessor ?? throw new ArgumentNullException(nameof(gradebookProcessor));
    }

    protected override async Task<TelegramResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken)
    {
        if (request.BackExecuted && !IsFirstHandler)
        {
            await ExecuteBackHandler();
            return new TelegramResponse();
        }

        var user = Context.User;

        await Client.RemoveReplyKeyboard(user);

        if (user.LastBotMessageId.HasValue)
        {
            await Client.DeleteMessageAsync(user.ChatId, user.LastBotMessageId.Value, cancellationToken: cancellationToken);
            user.LastBotMessageId = null;
            await UserService.UpdateUserAsync(user);
        }

        if (!string.IsNullOrWhiteSpace(request.Value))
        {
            UpdateSettingsItem(request.Value);

            if (!await ValidateUser())
            {
                await Client.SendTextMessageWithMarkup(user, "Неправильный логин или номер студ билета", Markups.DefaultMarkup);

                user.StudentCardId = null;
                user.Login = null;

                await UserService.UpdateUserAsync(user);

                return new TelegramResponse();
            }

            await UserService.UpdateUserAsync(user);

            if (!IsLastHandler)
            {
                await ExecuteNextHandler();
            }

            return new TelegramResponse();
        }

        var message = await Client.SendTextMessageWithMarkup(user, SettingsTitle, CreateMarkup());
        user.LastBotMessageId = message.MessageId;
        await UserService.UpdateUserAsync(user);

        return new TelegramResponse(TelegramResponseStatus.WaitingNextResponse, CallbackHandlerName);
    }

    protected abstract void UpdateSettingsItem(string value);

    protected virtual Task ExecuteNextHandler() => Task.CompletedTask;

    protected virtual Task ExecuteBackHandler() => Task.CompletedTask;

    protected virtual Task<bool> ValidateUser() => Task.FromResult(true);

    protected virtual InlineKeyboardMarkup CreateMarkup()
    {
        var buttons = new List<InlineKeyboardButton[]>
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData
                (
                    text: "Отмена",
                    callbackData: CallbackDataProcessor.CreateCallbackData(new CallbackData
                    {
                        Data = CommandKeys.EmptyData,
                        Handler = CommandKeys.Gradebook.Cancel,
                    })
                ), 
            }
        };

        if (!IsFirstHandler)
        {
            buttons.Add(new[]
            {
                InlineKeyboardButton.WithCallbackData
                (
                    text: "Назад",
                    callbackData: CallbackDataProcessor.CreateCallbackData(new CallbackData
                    {
                        Data = CommandKeys.Registrators.Back,
                        Handler = CallbackHandlerName,
                    })
                )
            });
        }

        return new InlineKeyboardMarkup(buttons);
    }
}