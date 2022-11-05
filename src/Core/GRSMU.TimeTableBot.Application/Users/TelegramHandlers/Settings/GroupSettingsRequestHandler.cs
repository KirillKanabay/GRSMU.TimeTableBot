using AutoMapper;
using GRSMU.TimeTableBot.Common.Broker.RequestBroker;
using GRSMU.TimeTableBot.Common.Contexts;
using GRSMU.TimeTableBot.Common.Extensions;
using GRSMU.TimeTableBot.Common.Services;
using GRSMU.TimeTableBot.Core.DataLoaders;
using GRSMU.TimeTableBot.Core.Immutable;
using GRSMU.TimeTableBot.Data.Users.Contracts;
using GRSMU.TimeTableBot.Domain.Users.Requests.Settings;
using Telegram.Bot;

namespace GRSMU.TimeTableBot.Application.Users.TelegramHandlers.Settings;

public class GroupSettingsRequestHandler : SettingsRequestHandlerBase<GroupSettingsRequestMessage>
{
    public GroupSettingsRequestHandler(ITelegramBotClient client, IRequestBroker requestBroker, IUserService userService, IMapper mapper, IUserRepository userRepository, FormDataLoader formDataLoader) : base(client, requestBroker, userService, mapper, userRepository, formDataLoader)
    {
    }

    protected override string SettingsTitle => "Укажи группу:";
    protected override int ItemsInRow => 3;
    protected override string CallbackHandlerName => CommandKeys.Registrators.Group;
    protected override bool IsFirstHandler => false;
    protected override bool IsLastHandler => true;

    protected override void UpdateSettingItem(UserContext user, string value) => user.GroupId = value;
    protected override Task<IReadOnlyDictionary<string, string>> GetSettingItems(UserContext user) => FormDataLoader.GetGroupsAsync(user.FacultyId, user.CourseId);
    protected override Task ExecuteNextHandler(UserContext user) => Task.CompletedTask;
    protected override Task ExecuteBackHandler(UserContext user) => RequestBroker.Publish(new FacultySettingsRequestMessage(user));
    
    protected override async Task PostExecuteAsync(GroupSettingsRequestMessage request, CancellationToken cancellationToken)
    {
        var user = request.UserContext;

        if (!string.IsNullOrWhiteSpace(request.Value) 
            && !string.IsNullOrWhiteSpace(user.GroupId) 
            && request.Value != CommandKeys.Registrators.Back)
        {
            if (user.LastBotMessageId.HasValue)
            {
                await Client.DeleteMessageAsync(user.ChatId, user.LastBotMessageId.Value, cancellationToken);
                await Client.SendTextMessageWithMarkup(user, "Настройка профиля завершена", Markups.DefaultMarkup);
                await UserService.DeleteLastMessageBotId(request.UserContext);
            }
        }
    }
}