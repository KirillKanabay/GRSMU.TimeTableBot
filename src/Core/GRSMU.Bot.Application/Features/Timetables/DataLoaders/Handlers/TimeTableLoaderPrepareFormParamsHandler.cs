using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using GRSMU.Bot.Application.Features.Timetables.DataLoaders.Common;
using GRSMU.Bot.Common.Extensions;
using GRSMU.Bot.Common.Telegram.Immutable;

namespace GRSMU.Bot.Application.Features.Timetables.DataLoaders.Handlers
{
    public class TimeTableLoaderPrepareFormParamsHandler : TimeTableLoaderHandlerBase
    {
        public override async Task Handle(ParserTimeTableContext context)
        {
            var config = Configuration.Default.WithDefaultLoader().WithDefaultCookies();
            IBrowsingContext browsingContext = BrowsingContext.New(config);
            IDocument queryDocument = await browsingContext.OpenAsync(context.Url);
            
            var form = queryDocument.QuerySelector<IHtmlFormElement>("form");
            
            context.FormParams.Upsert(RequestKeys.LastFocus, form.GetInputValue(RequestKeys.LastFocus));
            context.FormParams.Upsert(RequestKeys.ViewState, form.GetInputValue(RequestKeys.ViewState));
            context.FormParams.Upsert(RequestKeys.ViewStateGenerator, form.GetInputValue(RequestKeys.ViewStateGenerator));
            context.FormParams.Upsert(RequestKeys.EventValidation, form.GetInputValue(RequestKeys.EventValidation));
            context.FormParams.Upsert(RequestKeys.Faculty, context.Query.FacultyId);
            context.FormParams.Upsert(RequestKeys.DefaultDepartment);
            context.FormParams.Upsert(RequestKeys.Course, context.Query.CourseId);
            context.FormParams.Upsert(RequestKeys.Week, context.Query.Week);
            context.FormParams.Upsert(RequestKeys.DefaultButton);
            context.FormParams.Upsert(RequestKeys.DefaultFrame);

            var response = await form.SubmitAsync(context.FormParams, true);
            var responseForm = response.QuerySelector<IHtmlFormElement>("form");

            context.FormParams.Upsert(RequestKeys.LastFocus, responseForm.GetInputValue(RequestKeys.LastFocus));
            context.FormParams.Upsert(RequestKeys.ViewState, responseForm.GetInputValue(RequestKeys.ViewState));
            context.FormParams.Upsert(RequestKeys.ViewStateGenerator, responseForm.GetInputValue(RequestKeys.ViewStateGenerator));
            context.FormParams.Upsert(RequestKeys.EventValidation, responseForm.GetInputValue(RequestKeys.EventValidation));

            response = await responseForm.SubmitAsync(context.FormParams, true);
            responseForm = response.QuerySelector<IHtmlFormElement>("form");

            context.FormParams.Upsert(RequestKeys.LastFocus, responseForm.GetInputValue(RequestKeys.LastFocus));
            context.FormParams.Upsert(RequestKeys.ViewState, responseForm.GetInputValue(RequestKeys.ViewState));
            context.FormParams.Upsert(RequestKeys.ViewStateGenerator, responseForm.GetInputValue(RequestKeys.ViewStateGenerator));
            context.FormParams.Upsert(RequestKeys.EventValidation, responseForm.GetInputValue(RequestKeys.EventValidation));
            context.FormParams.Upsert(RequestKeys.Group, context.Query.GroupId);

            response = await responseForm.SubmitAsync(context.FormParams, true);

            context.Cookie = response.Cookie.CreateCookie();

            await base.Handle(context);
        }
    }
}
