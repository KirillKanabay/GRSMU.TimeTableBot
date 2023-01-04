using AutoMapper;
using GRSMU.Bot.Application.Features.Gradebooks.Helpers;
using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Contracts;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Common.Telegram.Immutable;
using GRSMU.Bot.Common.Telegram.Services;
using GRSMU.Bot.Domain.Gradebooks.TelegramRequests.Settings;
using Telegram.Bot;

namespace GRSMU.Bot.Application.Features.Gradebooks.TelegramHandlers.GradebookSettings;

public class SetGradebookPasswordRequestHandler : GradebookSettingsRequestHandlerBase<SetGradebookPasswordTelegramRequestMessage>
{
    public SetGradebookPasswordRequestHandler(
        ITelegramBotClient client, 
        ITelegramRequestContext context, 
        ITelegramRequestBroker requestBroker, 
        GradebookProcessor gradebookProcessor, 
        ITelegramUserService userService, 
        IMapper mapper) : base(client, context, requestBroker, userService, mapper, gradebookProcessor)
    {
    }

    protected override string SettingsTitle => "Введи свой номер студ билета. (Например: 11-12345)";
    protected override bool IsFirstHandler => false;
    protected override bool IsLastHandler => true;
    protected override string CallbackHandlerName => CommandKeys.Gradebook.SetPassword;

    protected override void UpdateSettingsItem(string value)
    {
        var user = Context.User;
        user.StudentCardId = value;
    }

    protected override Task<bool> ValidateUser()
    {
        var user = Context.User;
        return GradebookProcessor.TrySignInAsync(user);
    }

    protected override Task PostExecuteAsync(SetGradebookPasswordTelegramRequestMessage request, CancellationToken cancellationToken)
    {
        var user = Context.User;

        return user.IsStudentCardRegistered() 
            ? Client.SendTextMessageWithMarkup(user, "Регистрация студ билета успешно завершена!", Markups.GradebookMarkup) 
            : Task.CompletedTask;
    }

    protected override Task ExecuteBackHandler()
    {
        return RequestBroker.Publish(new SetGradebookLoginTelegramRequestMessage());
    }
}