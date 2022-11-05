using GRSMU.TimeTableBot.Common.Broker.RequestCache;
using GRSMU.TimeTableBot.Common.Broker.RequestFactories;
using GRSMU.TimeTableBot.Common.Common.Handlers.Data;
using GRSMU.TimeTableBot.Common.Extensions;
using GRSMU.TimeTableBot.Common.RequestMessages;
using GRSMU.TimeTableBot.Common.Services;
using GRSMU.TimeTableBot.Core.Immutable;
using GRSMU.TimeTableBot.Domain.Common.Requests;
using GRSMU.TimeTableBot.Domain.Reports.Requests;
using GRSMU.TimeTableBot.Domain.RequestMessages.Users;
using GRSMU.TimeTableBot.Domain.Timetables.Requests;
using GRSMU.TimeTableBot.Domain.Users.Requests.Settings;
using GRSMU.TimeTableBot.Domain.Users.TelegramRequests.Settings;
using Telegram.Bot.Types;

namespace GRSMU.TimeTableBot.Core;

public class RequestFactory : MappedRequestFactoryBase
{
    public RequestFactory(IRequestCache requestCache, IUserService userService) : base(requestCache, userService)
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
        var userContext = await UserService.CreateContextFromTelegramUpdateAsync(update);

        var value = CallbackDataProcessor.ReadCallbackData(update?.CallbackQuery?.Data).Data;

        var requestMessage = Activator.CreateInstance(typeof(TRequest), userContext) as TRequest;

        requestMessage.BackExecuted = value == CommandKeys.Registrators.Back;
        requestMessage.Value = value == CommandKeys.Registrators.Back ? string.Empty : value;

        return requestMessage;
    }

    #endregion

    private async Task<TelegramRequestMessageBase> CreateReportCommand(Update update, bool isCached)
    {
        var userContext = await UserService.CreateContextFromTelegramUpdateAsync(update);
        var requestMessage = new ReportRequestMessage(userContext);

        if (isCached)
        {
            requestMessage.Message = update.GetMessageText();
        }

        return requestMessage;
    }
}