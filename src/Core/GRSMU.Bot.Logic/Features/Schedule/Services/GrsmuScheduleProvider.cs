using System.Net;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp;
using GRSMU.Bot.Common;
using GRSMU.Bot.Common.Configurations;
using GRSMU.Bot.Common.Extensions;
using GRSMU.Bot.Logic.Features.Schedule.Dtos;
using GRSMU.Bot.Logic.Features.Schedule.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace GRSMU.Bot.Logic.Features.Schedule.Services;

public class GrsmuScheduleProvider : IScheduleProvider
{
    private GrsmuSourceConfiguration _srcConfiguration;
    private readonly IScheduleParser _parser;

    public GrsmuScheduleProvider(
        IOptions<GrsmuSourceConfiguration> srcConfiguration,
        IScheduleParser parser)
    {
        _parser = parser;
        _srcConfiguration = srcConfiguration.Value;
    }

    public async Task<List<ParsedScheduleDayDto>> GetScheduleByWeekAsync(DateTime week, string facultyId, string courseId, string groupId)
    {
        var rawPage = await GetRawPageAsync(week, facultyId, courseId, groupId);
        return await _parser.ParseAsync(rawPage);
    }

    private async Task<string> GetRawPageAsync(DateTime week, string facultyId, string courseId, string groupId)
    {
        var config = Configuration.Default.WithDefaultLoader().WithDefaultCookies();
        IBrowsingContext browsingContext = BrowsingContext.New(config);
        IDocument queryDocument = await browsingContext.OpenAsync(_srcConfiguration.TimetableUrl);

        var form = queryDocument.QuerySelector<IHtmlFormElement>("form");

        var formParams = new Dictionary<string, string>();

        formParams.Upsert(RequestKeys.LastFocus, form.GetInputValue(RequestKeys.LastFocus));
        formParams.Upsert(RequestKeys.ViewState, form.GetInputValue(RequestKeys.ViewState));
        formParams.Upsert(RequestKeys.ViewStateGenerator, form.GetInputValue(RequestKeys.ViewStateGenerator));
        formParams.Upsert(RequestKeys.EventValidation, form.GetInputValue(RequestKeys.EventValidation));
        formParams.Upsert(RequestKeys.Faculty, facultyId);
        formParams.Upsert(RequestKeys.DefaultDepartment);
        formParams.Upsert(RequestKeys.Course, courseId);
        formParams.Upsert(RequestKeys.Week, week.ToString("dd.MM.yyyy 0:00:00"));
        formParams.Upsert(RequestKeys.DefaultButton);
        formParams.Upsert(RequestKeys.DefaultFrame);

        var response = await form.SubmitAsync(formParams, true);
        var responseForm = response.QuerySelector<IHtmlFormElement>("form");

        formParams.Upsert(RequestKeys.LastFocus, responseForm.GetInputValue(RequestKeys.LastFocus));
        formParams.Upsert(RequestKeys.ViewState, responseForm.GetInputValue(RequestKeys.ViewState));
        formParams.Upsert(RequestKeys.ViewStateGenerator, responseForm.GetInputValue(RequestKeys.ViewStateGenerator));
        formParams.Upsert(RequestKeys.EventValidation, responseForm.GetInputValue(RequestKeys.EventValidation));

        response = await responseForm.SubmitAsync(formParams, true);
        responseForm = response.QuerySelector<IHtmlFormElement>("form");

        formParams.Upsert(RequestKeys.LastFocus, responseForm.GetInputValue(RequestKeys.LastFocus));
        formParams.Upsert(RequestKeys.ViewState, responseForm.GetInputValue(RequestKeys.ViewState));
        formParams.Upsert(RequestKeys.ViewStateGenerator, responseForm.GetInputValue(RequestKeys.ViewStateGenerator));
        formParams.Upsert(RequestKeys.EventValidation, responseForm.GetInputValue(RequestKeys.EventValidation));
        formParams.Upsert(RequestKeys.Group, groupId);
            
        response = await responseForm.SubmitAsync(formParams, true);
        var cookie = response.Cookie.CreateCookie();

        var cookieContainer = new CookieContainer();

        using (var handler = new HttpClientHandler { CookieContainer = cookieContainer })
        {
            using (var client = new HttpClient(handler))
            {
                var content = new FormUrlEncodedContent(formParams);
                cookieContainer.Add(new Uri(_srcConfiguration.CookieUrl), cookie);
                var result = await client.PostAsync(_srcConfiguration.TimetableUrl, content);

                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}