using GRSMU.Bot.Application.Features.Gradebooks.TelegramHandlers.GradebookSettings;
using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.RequestCache;
using GRSMU.Bot.Common.Telegram.Data;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Common.Telegram.Immutable;
using GRSMU.Bot.Common.Telegram.Models.Messages;
using GRSMU.Bot.Common.Telegram.RequestFactories;
using GRSMU.Bot.Common.Telegram.Services;
using GRSMU.Bot.Domain.Common.TelegramRequests;
using GRSMU.Bot.Domain.Gradebooks.TelegramRequests;
using GRSMU.Bot.Domain.Gradebooks.TelegramRequests.Settings;
using GRSMU.Bot.Domain.Reports.TelegramRequests;
using GRSMU.Bot.Domain.Timetables.TelegramRequests;
using GRSMU.Bot.Domain.Users.TelegramRequests.Settings;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GRSMU.Bot.Application;

public class RequestFactory : MappedRequestFactoryBase
{
    public RequestFactory(IRequestCache requestCache, ITelegramUserService userService, ITelegramRequestContext context) : base(requestCache, userService, context)
    {
        AddRequest<StartRequestMessage>(CommandKeys.Start);
        AddRequest<SetDefaultMenuRequestMessage>(CommandKeys.SetDefaultMenu);
        
        #region Reports

        AddRequest(CommandKeys.Reports.Report, CreateReportCommand);
        AddRequest<CancelReportRequestMessage>(CommandKeys.Reports.Cancel);

        #endregion

        #region TimeTable

        AddRequest<SetTimeTableKeyboardRequestMessage>(CommandKeys.ShowTimeTableCommand);
        AddRequest<GetTodayTimeTableRequestMessage>(CommandKeys.TimeTable.Today);
        AddRequest<GetTomorrowTimeTableRequestMessage>(CommandKeys.TimeTable.Tomorrow);
        AddRequest<GetWeekTimeTableRequestMessage>(CommandKeys.TimeTable.Week);
        AddRequest<GetNextWeekTimeTableRequestMessage>(CommandKeys.TimeTable.NextWeek);

        #endregion

        #region Registrators

        AddRequest<RunSettingsRequestMessage>(CommandKeys.Registrators.Run);
        AddRequest(CommandKeys.Registrators.Course, CreateSettingsCommand<CourseSettingsRequestMessage>);
        AddRequest(CommandKeys.Registrators.Faculty, CreateSettingsCommand<FacultySettingsRequestMessage>);
        AddRequest(CommandKeys.Registrators.Group, CreateSettingsCommand<GroupSettingsRequestMessage>);

        #endregion

        AddRequest<SetGradebookKeyboardTelegramRequestMessage>(CommandKeys.Gradebook.Run);
        AddRequest(CommandKeys.Gradebook.SetLogin, CreateGradeBookSettingsCommand<SetGradebookLoginTelegramRequestMessage>);
        AddRequest(CommandKeys.Gradebook.SetPassword, CreateGradeBookSettingsCommand<SetGradebookPasswordTelegramRequestMessage>);
        AddRequest<CancelGradebookRegistrationTelegramRequestMessage>(CommandKeys.Gradebook.Cancel);
    }
    
    #region Registrators

    private async Task<TelegramCommandMessageBase> CreateSettingsCommand<TRequest>(Update update, bool isCached)
        where TRequest : SettingsRequestMessageBase
    {
        var value = CallbackDataProcessor.ReadCallbackData(update?.CallbackQuery?.Data).Data;

        var requestMessage = Activator.CreateInstance(typeof(TRequest)) as TRequest;

        requestMessage.BackExecuted = value == CommandKeys.Registrators.Back;
        requestMessage.Value = value == CommandKeys.Registrators.Back ? string.Empty : value;

        return requestMessage;
    }

    #endregion

    private async Task<TelegramCommandMessageBase> CreateReportCommand(Update update, bool isCached)
    {
        var requestMessage = new ReportRequestMessage();

        if (isCached)
        {
            requestMessage.Message = update.GetMessageText();
        }

        return requestMessage;
    }

    private async Task<TelegramCommandMessageBase> CreateGradeBookSettingsCommand<TCommand>(Update update, bool isCached)
        where TCommand : GradebookSettingsTelegramRequestMessageBase, new()
    {
        var requestMessage = new TCommand();

        if (update.Type == UpdateType.CallbackQuery)
        {
            var value = CallbackDataProcessor.ReadCallbackData(update?.CallbackQuery?.Data).Data;
            requestMessage.BackExecuted = value == CommandKeys.Gradebook.Back;
        }

        if (isCached)
        {
            requestMessage.Value = update.GetMessageText();
        }

        return requestMessage;
    }
}