using AutoMapper;
using GRSMU.Bot.Common.Broker.Contracts;
using GRSMU.Bot.Common.Contexts;
using GRSMU.Bot.Common.Telegram.Services;
using GRSMU.Bot.Core.DataLoaders;
using GRSMU.Bot.Core.Immutable;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Domain.Users.TelegramRequests.Settings;
using Telegram.Bot;

namespace GRSMU.Bot.Application.Users.TelegramHandlers.Settings;

public class FacultySettingsRequestHandler : SettingsRequestHandlerBase<FacultySettingsRequestMessage>
{
    public FacultySettingsRequestHandler(ITelegramBotClient client, IRequestBroker requestBroker, ITelegramUserService userService, IMapper mapper, IUserRepository userRepository, FormDataLoader formDataLoader) : base(client, requestBroker, userService, mapper, userRepository, formDataLoader)
    {
    }

    protected override string SettingsTitle => "Укажи факультет:";
    protected override string CallbackHandlerName => CommandKeys.Registrators.Faculty;
    protected override bool IsFirstHandler => false;
    protected override bool IsLastHandler => false;

    protected override void UpdateSettingItem(UserContext user, string value) => user.FacultyId = value;
    protected override Task<IReadOnlyDictionary<string, string>> GetSettingItems(UserContext user) => FormDataLoader.GetFacultiesAsync();
    protected override Task ExecuteNextHandler(UserContext user) => RequestBroker.Publish(new GroupSettingsRequestMessage(user));
    protected override Task ExecuteBackHandler(UserContext user) => RequestBroker.Publish(new CourseSettingsRequestMessage(user));
}