using GRSMU.Bot.Common;
using GRSMU.Bot.Common.Configurations;
using GRSMU.Bot.Common.Extensions;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Logic.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace GRSMU.Bot.Logic.Features.Gradebook.Services;

public class GrsmuGradebookProvider : IGradebookProvider
{
    private readonly GrsmuSourceConfiguration _srcConfiguration;
    private readonly IGradebookParser _gradebookParser;

    public GrsmuGradebookProvider(
        IOptions<GrsmuSourceConfiguration> srcConfiguration,
        IGradebookParser gradebookParser)
    {
        _gradebookParser = gradebookParser;
        _srcConfiguration = srcConfiguration.Value;
    }

    public async Task<ExecutionResult<GradebookSignInResultDto>> CheckSignInResultAsync(StudentCardIdDto studentCardId)
    {
        var rawPage = await GetRawPageAsync(studentCardId.Login, studentCardId.Password);
        return await _gradebookParser.ParseSignInResultAsync(rawPage);
    }

    public async Task<ExecutionResult<List<GradebookDto>>> GetUserGradebookAsync(StudentCardIdDto studentCardId)
    {
        var rawPage = await GetRawPageAsync(studentCardId.Login, studentCardId.Password);
        return await _gradebookParser.ParseGradebookAsync(rawPage);
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

