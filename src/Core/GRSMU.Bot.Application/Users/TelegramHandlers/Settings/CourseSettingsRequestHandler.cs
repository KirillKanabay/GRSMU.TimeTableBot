using AutoMapper;
using GRSMU.Bot.Common.Broker.RequestBroker;
using GRSMU.Bot.Common.Contexts;
using GRSMU.Bot.Common.Services;
using GRSMU.Bot.Core.DataLoaders;
using GRSMU.Bot.Core.Immutable;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Domain.Users.TelegramRequests.Settings;
using Telegram.Bot;

namespace GRSMU.Bot.Application.Users.TelegramHandlers.Settings;

public class CourseSettingsRequestHandler : SettingsRequestHandlerBase<CourseSettingsRequestMessage>
{
    public CourseSettingsRequestHandler(ITelegramBotClient client, IRequestBroker requestBroker, IUserService userService, IMapper mapper, IUserRepository userRepository, FormDataLoader formDataLoader) : base(client, requestBroker, userService, mapper, userRepository, formDataLoader)
    {
    }

    protected override string SettingsTitle => "Укажи курс обучения:";
    protected override string CallbackHandlerName => CommandKeys.Registrators.Course;
    protected override int ItemsInRow => 2;
    protected override bool IsFirstHandler => true;
    protected override bool IsLastHandler => false;

    protected override void UpdateSettingItem(UserContext user, string value) => user.CourseId = value;
    protected override Task<IReadOnlyDictionary<string, string>> GetSettingItems(UserContext user) => FormDataLoader.GetCoursesAsync();
    protected override Task ExecuteNextHandler(UserContext user) => RequestBroker.Publish(new FacultySettingsRequestMessage(user));
    protected override Task ExecuteBackHandler(UserContext user) => Task.CompletedTask;
}