using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using GRSMU.Bot.Common;
using GRSMU.Bot.Common.Configurations;
using GRSMU.Bot.Common.Extensions;
using GRSMU.Bot.Logic.Dtos;
using GRSMU.Bot.Logic.Features.Faculty.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GRSMU.Bot.Logic.Features.Faculty.Services;

public class GrsmuFacultyInfoProvider : IFacultyInfoProvider
{
    private readonly GrsmuSourceConfiguration _srcConfiguration;
    private readonly ILogger<GrsmuFacultyInfoProvider> _logger;

    public GrsmuFacultyInfoProvider(
        IOptions<GrsmuSourceConfiguration> srcConfiguration, 
        ILogger<GrsmuFacultyInfoProvider> logger)
    {
        _srcConfiguration = srcConfiguration.Value;
        _logger = logger;
    }

    public Task<List<LookupDto>> GetFacultyLookupAsync()
    {
        return LookupAsync($"#{RequestKeys.Faculty}");
    }

    public Task<List<LookupDto>> GetCoursesLookupAsync()
    {
        return LookupAsync($"#{RequestKeys.Course}");
    }

    public async Task<List<LookupDto>> GetGroupsLookupAsync(string facultyId, string courseId)
    {
        var form = await GetFormAsync();

        if (form is null)
        {
            return [];
        }

        var formParams = new Dictionary<string, string>();

        var formWeek = LookupForm($"#{RequestKeys.Week}", form).FirstOrDefault(x =>
        {
            var weekDate = DateTime.Parse(x.Value).StartOfDay();
            var dateNow = DateTime.UtcNow;

            return dateNow.StartOfWeek() == weekDate;
        });

        if (formWeek is null)
        {
            _logger.LogWarning("Can't find current week, while groups lookup");
            return [];
        }

        formParams.Upsert(RequestKeys.LastFocus, form.GetInputValue(RequestKeys.LastFocus));
        formParams.Upsert(RequestKeys.ViewState, form.GetInputValue(RequestKeys.ViewState));
        formParams.Upsert(RequestKeys.ViewStateGenerator, form.GetInputValue(RequestKeys.ViewStateGenerator));
        formParams.Upsert(RequestKeys.EventValidation, form.GetInputValue(RequestKeys.EventValidation));
        formParams.Upsert(RequestKeys.Faculty, facultyId);
        formParams.Upsert(RequestKeys.DefaultDepartment);
        formParams.Upsert(RequestKeys.Course, courseId);
        formParams.Upsert(RequestKeys.Week, formWeek.Id);
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

        if (responseForm is null)
        {
            _logger.LogWarning("Can't find group submitted form.");
            return [];
        }

        return LookupForm($"#{RequestKeys.Group}", responseForm);
    }

    public Task<List<LookupDto>> GetWeeksLookupAsync()
    {
        return LookupAsync($"#{RequestKeys.Week}");
    }

    private async Task<List<LookupDto>> LookupAsync(string fieldSelector)
    {
        var form = await GetFormAsync();
        return form is null ? [] : LookupForm(fieldSelector, form);
    }

    private List<LookupDto> LookupForm(string fieldSelector, IHtmlFormElement form)
    {
        var selectEl = form.QuerySelector<IHtmlSelectElement>(fieldSelector);
        return selectEl?.Options.Select(opt => new LookupDto(opt.Value, opt.Text)).ToList() ?? [];
    }

    private async Task<IHtmlFormElement?> GetFormAsync()
    {
        var config = Configuration.Default.WithDefaultLoader().WithDefaultCookies();
        IBrowsingContext context = BrowsingContext.New(config);
        IDocument queryDocument = await context.OpenAsync(_srcConfiguration.TimetableUrl);
        
        var form = queryDocument.QuerySelector<IHtmlFormElement>("form");

        return form;
    }
}