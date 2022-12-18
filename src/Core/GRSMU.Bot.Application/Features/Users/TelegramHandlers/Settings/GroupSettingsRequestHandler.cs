using AutoMapper;
using GRSMU.Bot.Application.Features.Timetables.DataLoaders;
using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Contracts;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Common.Telegram.Models;
using GRSMU.Bot.Common.Telegram.Services;
using GRSMU.Bot.Core.Immutable;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Domain.Users.TelegramRequests.Settings;
using Telegram.Bot;

namespace GRSMU.Bot.Application.Features.Users.TelegramHandlers.Settings;

public class GroupSettingsRequestHandler : SettingsRequestHandlerBase<GroupSettingsRequestMessage>
{
    public GroupSettingsRequestHandler(
        ITelegramBotClient client, 
        ITelegramRequestBroker requestBroker, 
        ITelegramUserService userService, 
        IMapper mapper, 
        IUserRepository userRepository, 
        FormDataLoader formDataLoader,
        ITelegramRequestContext context) : base(client, requestBroker, userService, mapper, userRepository, formDataLoader, context)
    {
    }

    protected override string SettingsTitle => "Укажи группу:";
    protected override int ItemsInRow => 3;
    protected override string CallbackHandlerName => CommandKeys.Registrators.Group;
    protected override bool IsFirstHandler => false;
    protected override bool IsLastHandler => true;

    protected override void UpdateSettingItem(TelegramUser user, string value) => user.GroupId = value;
    protected override Task<IReadOnlyDictionary<string, string>> GetSettingItems(TelegramUser user) => FormDataLoader.GetGroupsAsync(user.FacultyId, user.CourseId);
    protected override Task ExecuteNextHandler(TelegramUser user) => Task.CompletedTask;
    protected override Task ExecuteBackHandler(TelegramUser user) => RequestBroker.Publish(new FacultySettingsRequestMessage());

    protected override async Task PostExecuteAsync(GroupSettingsRequestMessage request, CancellationToken cancellationToken)
    {
        var user = Context.User;

        if (!string.IsNullOrWhiteSpace(request.Value)
            && !string.IsNullOrWhiteSpace(user.GroupId)
            && request.Value != CommandKeys.Registrators.Back)
        {
            if (user.LastBotMessageId.HasValue)
            {
                await Client.DeleteMessageAsync(user.ChatId, user.LastBotMessageId.Value, cancellationToken);
                await Client.SendTextMessageWithMarkup(user, "Настройка профиля завершена", Markups.DefaultMarkup);
                await UserService.DeleteLastMessageBotIdAsync(user);
            }
        }
    }
}