using AutoMapper;
using GRSMU.Bot.Application.Features.Timetables.DataLoaders;
using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Contracts;
using GRSMU.Bot.Common.Telegram.Models;
using GRSMU.Bot.Common.Telegram.Services;
using GRSMU.Bot.Core.Immutable;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Domain.Users.TelegramRequests.Settings;
using Telegram.Bot;

namespace GRSMU.Bot.Application.Features.Users.TelegramHandlers.Settings;

public class FacultySettingsRequestHandler : SettingsRequestHandlerBase<FacultySettingsRequestMessage>
{
    public FacultySettingsRequestHandler(
        ITelegramBotClient client, 
        ITelegramRequestBroker requestBroker, 
        ITelegramUserService userService, 
        IMapper mapper, 
        IUserRepository userRepository, 
        FormDataLoader formDataLoader,
        ITelegramRequestContext context) : base(client, requestBroker, userService, mapper, userRepository, formDataLoader, context)
    {
    }

    protected override string SettingsTitle => "Укажи факультет:";
    protected override string CallbackHandlerName => CommandKeys.Registrators.Faculty;
    protected override bool IsFirstHandler => false;
    protected override bool IsLastHandler => false;

    protected override void UpdateSettingItem(TelegramUser user, string value) => user.FacultyId = value;
    protected override Task<IReadOnlyDictionary<string, string>> GetSettingItems(TelegramUser user) => FormDataLoader.GetFacultiesAsync();
    protected override Task ExecuteNextHandler(TelegramUser user) => RequestBroker.Publish(new GroupSettingsRequestMessage());
    protected override Task ExecuteBackHandler(TelegramUser user) => RequestBroker.Publish(new CourseSettingsRequestMessage());
}