using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using GRSMU.Bot.Common.Configurations;
using GRSMU.Bot.Common.Extensions;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Common.Telegram.Immutable;
using GRSMU.Bot.Logic.Features.Gradebook.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Services.Interfaces;
using GRSMU.Bot.Logic.Immutable;
using Microsoft.Extensions.Options;

namespace GRSMU.Bot.Logic.Features.Gradebook.Services;

public class GrsmuGradebookProvider : IGradebookProvider
{
    private const string GradebookInvalidLoginOrPassword = "\n\t\talert(\"Неправильный логин или номер студенческого\");\n\t";

    private readonly GrsmuSourceConfiguration _srcConfiguration;
    
    public GrsmuGradebookProvider(IOptions<GrsmuSourceConfiguration> srcConfiguration)
    {
        _srcConfiguration = srcConfiguration.Value;
    }

    public async Task<ExecutionResult<GradebookSignInResultDto>> CheckSignInResultAsync(string login, string password)
    {
        var document = await GetHtmlDocumentAsync(login, password);
        
        if (!CheckSignIn(document))
        {
            return ExecutionResult.Failure<GradebookSignInResultDto>(Error.ValidationError(ErrorResourceKeys.GradebookSignInFailed));
        }

        var signInResult = GetSignInResult(document);

        if (signInResult is null)
        {
            return ExecutionResult.Failure<GradebookSignInResultDto>(Error.ValidationError(ErrorResourceKeys.GradebookSignInFailed));
        }

        return ExecutionResult.Success(signInResult);
    }

    private async Task<IHtmlDocument> GetHtmlDocumentAsync(string login, string password)
    {
        var rawPage = await GetRawPageAsync(login, password);

        var parser = new HtmlParser(new HtmlParserOptions
        {
            IsNotConsumingCharacterReferences = true
        });

        return await parser.ParseDocumentAsync(rawPage, CancellationToken.None);
    } 

    private bool CheckSignIn(IHtmlDocument document)
    {
        var scripts = document.QuerySelectorAll<IHtmlScriptElement>("script");

        return !scripts.Any(x => x.Text.Equals(GradebookInvalidLoginOrPassword));
    }

    private GradebookSignInResultDto? GetSignInResult(IHtmlDocument document)
    {
        var h1List = document.QuerySelectorAll<IHtmlHeadingElement>("h1");

        var regex = new Regex("курс-");

        var heading = h1List.Select(x => x.Text()).FirstOrDefault(regex.IsMatch);

        if (string.IsNullOrWhiteSpace(heading))
        {
            return null;
        }

        var match = Regex.Match(heading, @"^(?<fullname>.+?)-(?<courseId>\d+)\sкурс-(?<groupId>\d+)\sгруппа$");

        if (!match.Success)
        {
            throw new ArgumentException("Неверный формат строки.");
        }

        return new GradebookSignInResultDto(
            match.Groups["fullname"].Value.Trim(),
            match.Groups["courseId"].Value.Trim(),
            match.Groups["groupId"].Value.Trim()
        );
    }

    private async Task<string> GetRawPageAsync(string login, string password)
    {
        var formParams = new Dictionary<string, string>();

        formParams.Upsert(RequestKeys.Login, login);
        formParams.Upsert(RequestKeys.Password, password);

        using (var client = new HttpClient())
        {
            var content = new FormUrlEncodedContent(formParams);
            var result = await client.PostAsync(_srcConfiguration.GradebookUrl, content);

            return await result.Content.ReadAsStringAsync();
        }
    }
}

