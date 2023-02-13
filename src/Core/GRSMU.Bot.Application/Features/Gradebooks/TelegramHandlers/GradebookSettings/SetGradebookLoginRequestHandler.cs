using AutoMapper;
using GRSMU.Bot.Application.Features.Gradebooks.Helpers;
using GRSMU.Bot.Application.Features.Timetables.DataLoaders;
using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Contracts;
using GRSMU.Bot.Common.Telegram.Immutable;
using GRSMU.Bot.Common.Telegram.Services;
using GRSMU.Bot.Domain.Gradebooks.TelegramRequests.Settings;
using Telegram.Bot;

namespace GRSMU.Bot.Application.Features.Gradebooks.TelegramHandlers.GradebookSettings;

public class SetGradebookLoginRequestHandler : GradebookSettingsRequestHandlerBase<SetGradebookLoginTelegramRequestMessage>
{
    public SetGradebookLoginRequestHandler(
        ITelegramBotClient client, 
        ITelegramRequestContext context, 
        ITelegramRequestBroker requestBroker, 
        GradebookProcessor gradebookProcessor, 
        ITelegramUserService userService, 
        IMapper mapper) : base(client, context, requestBroker, userService, mapper, gradebookProcessor)
    {
    }

    protected override string SettingsTitle => "Введи свой логин, который указан на студенченском. (Например:ivanovivan)";
    protected override bool IsFirstHandler => true;
    protected override bool IsLastHandler => false;
    protected override string CallbackHandlerName => CommandKeys.Gradebook.SetLogin;
    
    protected override void UpdateSettingsItem(string value)
    {
        var user = Context.User;
        user.Login = value;
    }

    protected override Task ExecuteNextHandler() => RequestBroker.Publish(new SetGradebookPasswordTelegramRequestMessage());
}