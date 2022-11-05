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