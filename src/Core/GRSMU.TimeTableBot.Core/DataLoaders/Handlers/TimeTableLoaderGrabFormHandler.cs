using System.Net;
using GRSMU.TimeTableBot.Core.DataLoaders.Common;

namespace GRSMU.TimeTableBot.Core.DataLoaders.Handlers
{
    public class TimeTableLoaderGrabFormHandler : TimeTableLoaderHandlerBase
    {
        public override async Task Handle(ParserTimeTableContext query)
        {
            var cookieContainer = new CookieContainer();
            
            using (var handler = new HttpClientHandler { CookieContainer = cookieContainer })
            {
                using (var client = new HttpClient(handler))
                {
                    var content = new FormUrlEncodedContent(query.FormParams);
                    cookieContainer.Add(new Uri("https://rsp-med.grsu.by"), query.Cookie);
                    var result = await client.PostAsync(query.Url, content);

                    query.HtmlContent = await result.Content.ReadAsStringAsync();
                }
            }

            await base.Handle(query);
        }
    }
}
