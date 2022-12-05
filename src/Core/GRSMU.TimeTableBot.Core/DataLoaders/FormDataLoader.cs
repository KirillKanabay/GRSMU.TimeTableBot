using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp;
using GRSMU.TimeTableBot.Common.Extensions;
using GRSMU.TimeTableBot.Common.Models.Options;
using GRSMU.TimeTableBot.Core.Immutable;

namespace GRSMU.TimeTableBot.Core.DataLoaders
{
    public class FormDataLoader
    {
        private readonly SourceOptions _sourceOptions;

        public FormDataLoader(SourceOptions sourceOptions)
        {
            _sourceOptions = sourceOptions ?? throw new ArgumentNullException(nameof(sourceOptions));
        }

        #region Courses

        private IReadOnlyDictionary<string, string> _cachedCourses;
        public async Task<IReadOnlyDictionary<string, string>> GetCoursesAsync()
        {
            if (_cachedCourses != null)
            {
                return _cachedCourses;
            }

            var config = Configuration.Default.WithDefaultLoader().WithDefaultCookies();
            IBrowsingContext context = BrowsingContext.New(config);
            IDocument queryDocument = await context.OpenAsync(_sourceOptions.Url);
            var form = queryDocument.QuerySelector<IHtmlFormElement>("form");

            var courseSelect = form.QuerySelector<IHtmlSelectElement>($"#{RequestKeys.Course}");

            var courses = courseSelect.Options.ToDictionary(k => k.Text, v => v.Value);

            _cachedCourses = courses;

            return courses;
        }

        #endregion

        #region Faculties

        private IReadOnlyDictionary<string, string> _cachedFaculties;
        public async Task<IReadOnlyDictionary<string, string>> GetFacultiesAsync()
        {
            if (_cachedFaculties != null)
            {
                return _cachedFaculties;
            }

            var config = Configuration.Default.WithDefaultLoader().WithDefaultCookies();
            IBrowsingContext context = BrowsingContext.New(config);
            IDocument queryDocument = await context.OpenAsync(_sourceOptions.Url);
            var form = queryDocument.QuerySelector<IHtmlFormElement>("form");

            var facultySelect = form.QuerySelector<IHtmlSelectElement>($"#{RequestKeys.Faculty}");

            var faculties = facultySelect.Options.ToDictionary(k => k.Text, v => v.Value);

            _cachedFaculties = faculties;

            return faculties;
        }

        #endregion

        #region Groups

        private readonly Dictionary<(string, string), IReadOnlyDictionary<string, string>> _cachedGroups = new();
        public async Task<IReadOnlyDictionary<string, string>> GetGroupsAsync(string facultyId, string courseId)
        {
            if (_cachedGroups.TryGetValue((facultyId, courseId), out var cachedGroups))
            {
                return cachedGroups;
            }

            var config = Configuration.Default.WithDefaultLoader().WithDefaultCookies();
            IBrowsingContext context = BrowsingContext.New(config);
            IDocument queryDocument = await context.OpenAsync(_sourceOptions.Url);
            
            var form = queryDocument.QuerySelector<IHtmlFormElement>("form");

            var formParams = new Dictionary<string, string>();

            formParams.Upsert(RequestKeys.LastFocus, form.GetInputValue(RequestKeys.LastFocus));
            formParams.Upsert(RequestKeys.ViewState, form.GetInputValue(RequestKeys.ViewState));
            formParams.Upsert(RequestKeys.ViewStateGenerator, form.GetInputValue(RequestKeys.ViewStateGenerator));
            formParams.Upsert(RequestKeys.EventValidation, form.GetInputValue(RequestKeys.EventValidation));
            formParams.Upsert(RequestKeys.Faculty, facultyId);
            formParams.Upsert(RequestKeys.DefaultDepartment);
            formParams.Upsert(RequestKeys.Course, courseId);
            formParams.Upsert(RequestKeys.Week, "03.10.2022 0:00:00");
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

            var groupSelect = responseForm.QuerySelector<IHtmlSelectElement>($"#{RequestKeys.Group}");

            var groups = groupSelect.Options.ToDictionary(k => k.Text, v => v.Value);

            _cachedGroups.TryAdd((facultyId, courseId), groups);

            return groups;
        }

        #endregion

        #region Dates

        private IReadOnlyDictionary<DateTime, string> _cachedWeeks;
        public async Task<IReadOnlyDictionary<DateTime, string>> GetWeeksAsync()
        {
            if (_cachedWeeks != null)
            {
                return _cachedWeeks;
            }

            var config = Configuration.Default.WithDefaultLoader().WithDefaultCookies();
            IBrowsingContext context = BrowsingContext.New(config);
            IDocument queryDocument = await context.OpenAsync(_sourceOptions.Url);
            var form = queryDocument.QuerySelector<IHtmlFormElement>("form");

            var weekSelect = form.QuerySelector<IHtmlSelectElement>($"#{RequestKeys.Week}");

            var weeks = weekSelect.Options.ToDictionary(k => DateTime.Parse(k.Text), v => v.Value);

            _cachedWeeks = weeks;

            return weeks;
        }


        #endregion
    }
}
