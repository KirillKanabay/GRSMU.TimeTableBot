using AutoMapper;
using GRSMU.Bot.Common.Broker.Contracts;
using GRSMU.Bot.Common.Contexts;
using GRSMU.Bot.Common.Services;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Core.DataLoaders;
using GRSMU.Bot.Core.Immutable;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Domain.Users.TelegramRequests.Settings;
using Telegram.Bot;

namespace GRSMU.Bot.Application.Features.Users.TelegramHandlers.Settings;

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