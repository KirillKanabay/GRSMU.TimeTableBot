using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using GRSMU.Bot.Common.Extensions;
using GRSMU.Bot.Common.Models.Options;
using GRSMU.Bot.Common.Telegram.Immutable;

namespace GRSMU.Bot.Application.Features.Gradebooks.Helpers
{
    public class GradebookProcessor
    {
        private readonly SourceOptions _options;

        public GradebookProcessor(SourceOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<bool> TrySignInAsync(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new ArgumentNullException(nameof(login));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            var rawPage = await GetRawPageAsync(login, password);

            if (rawPage == null)
            {
                return false;
            }

            var parser = new HtmlParser(new HtmlParserOptions
            {
                IsNotConsumingCharacterReferences = true
            });

            var document = await parser.ParseDocumentAsync(rawPage, CancellationToken.None);

            var scripts = document.QuerySelectorAll<IHtmlScriptElement>("script");

            return !scripts.Any(x => x.Text.Equals(RequestKeys.GradebookInvalidLoginOrPassword));
        }

        private async Task<string> GetRawPageAsync(string login, string password)
        {
            var formParams = new Dictionary<string, string>();

            formParams.Upsert(RequestKeys.Login, login);
            formParams.Upsert(RequestKeys.Password, password);

            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(formParams);
                var result = await client.PostAsync(_options.GradebookUrl, content);

                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}
