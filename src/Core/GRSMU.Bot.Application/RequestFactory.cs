using GRSMU.Bot.Common.Telegram.Brokers.RequestCache;
using GRSMU.Bot.Common.Telegram.Data;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Common.Telegram.RequestFactories;
using GRSMU.Bot.Common.Telegram.Services;
using GRSMU.Bot.Core.Immutable;
using GRSMU.Bot.Domain.Common.Requests;
using GRSMU.Bot.Domain.Reports.Requests;
using GRSMU.Bot.Domain.Timetables.Requests;
using GRSMU.Bot.Domain.Users.TelegramRequests.Settings;
using Telegram.Bot.Types;

namespace GRSMU.Bot.Core;

public class RequestFactory : MappedRequestFactoryBase
{
    public RequestFactory(IRequestCache requestCache, ITelegramUserService userService) : base(requestCache, userService)
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
    }
    
    #region Registrators

    private async Task<TelegramRequestMessageBase> CreateSettingsCommand<TRequest>(Update update, bool isCached)
        where TRequest : SettingsRequestMessageBase
    {
        var userContext = await UserService.CreateUserFromTelegramUpdateAsync(update);

        var value = CallbackDataProcessor.ReadCallbackData(update?.CallbackQuery?.Data).Data;

        var requestMessage = Activator.CreateInstance(typeof(TRequest), userContext) as TRequest;

        requestMessage.BackExecuted = value == CommandKeys.Registrators.Back;
        requestMessage.Value = value == CommandKeys.Registrators.Back ? string.Empty : value;

        return requestMessage;
    }

    #endregion

    private async Task<TelegramRequestMessageBase> CreateReportCommand(Update update, bool isCached)
    {
        var userContext = await UserService.CreateUserFromTelegramUpdateAsync(update);
        var requestMessage = new ReportRequestMessage(userContext);

        if (isCached)
        {
            requestMessage.Message = update.GetMessageText();
        }

        return requestMessage;
    }
}