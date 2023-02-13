using System.Net;
using GRSMU.Bot.Application.Features.Timetables.DataLoaders.Common;

namespace GRSMU.Bot.Application.Features.Timetables.DataLoaders.Handlers
{
    public class TimeTableLoaderGrabFormHandler : TimeTableLoaderHandlerBase
    {
        private const string CookieUrl = "https://rsp-med.grsu.by";

        public override async Task Handle(ParserTimeTableContext query)
        {
            var cookieContainer = new CookieContainer();
            
            using (var handler = new HttpClientHandler { CookieContainer = cookieContainer })
            {
                using (var client = new HttpClient(handler))
                {
                    var content = new FormUrlEncodedContent(query.FormParams);
                    cookieContainer.Add(new Uri(CookieUrl), query.Cookie);
                    var result = await client.PostAsync(query.Url, content);

                    query.HtmlContent = await result.Content.ReadAsStringAsync();
                }
            }

            await base.Handle(query);
        }
    }
}
