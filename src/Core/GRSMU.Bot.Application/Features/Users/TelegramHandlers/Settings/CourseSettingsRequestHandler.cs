using AutoMapper;
using GRSMU.Bot.Application.Features.Timetables.DataLoaders;
using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Contracts;
using GRSMU.Bot.Common.Telegram.Immutable;
using GRSMU.Bot.Common.Telegram.Models;
using GRSMU.Bot.Common.Telegram.Services;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Domain.Users.TelegramRequests.Settings;
using Telegram.Bot;

namespace GRSMU.Bot.Application.Features.Users.TelegramHandlers.Settings;

public class CourseSettingsRequestHandler : SettingsRequestHandlerBase<CourseSettingsRequestMessage>
{
    public CourseSettingsRequestHandler(
        ITelegramBotClient client, 
        ITelegramRequestBroker requestBroker, 
        ITelegramUserService userService, 
        IMapper mapper, 
        IUserRepository userRepository, 
        FormDataLoader formDataLoader,
        ITelegramRequestContext context) : base(client, requestBroker, userService, mapper, userRepository, formDataLoader, context)
    {
    }

    protected override string SettingsTitle => "Укажи курс обучения:";
    protected override string CallbackHandlerName => CommandKeys.Registrators.Course;
    protected override int ItemsInRow => 2;
    protected override bool IsFirstHandler => true;
    protected override bool IsLastHandler => false;

    protected override void UpdateSettingItem(TelegramUser user, string value) => user.CourseId = value;
    protected override Task<IReadOnlyDictionary<string, string>> GetSettingItems(TelegramUser user) => FormDataLoader.GetCoursesAsync();
    protected override Task ExecuteNextHandler(TelegramUser user) => RequestBroker.Publish(new FacultySettingsRequestMessage());
    protected override Task ExecuteBackHandler(TelegramUser user) => Task.CompletedTask;
}