using AutoMapper;
using GRSMU.TimeTableBot.Common.Broker.RequestBroker;
using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.Services;
using GRSMU.TimeTableBot.Core.DataLoaders;
using GRSMU.TimeTableBot.Core.Immutable;
using GRSMU.TimeTableBot.Data.Users.Contracts;
using GRSMU.TimeTableBot.Domain.RequestMessages.Users.Settings;
using Telegram.Bot;

namespace GRSMU.TimeTableBot.Application.Users.Handlers.Settings;

public class FacultySettingsRequestHandler : SettingsRequestHandlerBase<FacultySettingsRequestMessage>
{
    public FacultySettingsRequestHandler(ITelegramBotClient client, IRequestBroker requestBroker, IUserService userService, IMapper mapper, IUserRepository userRepository, FormDataLoader formDataLoader) : base(client, requestBroker, userService, mapper, userRepository, formDataLoader)
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