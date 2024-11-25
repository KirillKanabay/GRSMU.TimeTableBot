using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;
using GRSMU.Bot.Common.Enums;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Web.Core.Configurations;
using GRSMU.Bot.Web.Core.Models;
using GRSMU.Bot.Web.Core.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace GRSMU.Bot.Web.Core.Services
{
    internal class TelegramTokenValidator : ITelegramTokenValidator
    {
        private readonly TelegramConfiguration _configuration;

        public TelegramTokenValidator(IOptions<TelegramConfiguration> telegramConfig)
        {
            _configuration = telegramConfig.Value;
        }

        public ExecutionResult<TelegramAppUser> Validate(string token)
        {
            var decodedString = HttpUtility.UrlDecode(token);
            var queryParams = HttpUtility.ParseQueryString(decodedString);

            if (!IsValidToken(queryParams))
            {
                return ExecutionResult.Failure<TelegramAppUser>(new Error(
                    "TelegramToken.Invalid", "Telegram token is invalid", ErrorType.Validation));
            }

            var user = CreateUser(queryParams["user"]);

            if (user is null)
            {
                return ExecutionResult.Failure<TelegramAppUser>(new Error(
                    "TelegramToken.Invalid", "Can't deserilize user info", ErrorType.Problem));
            }

            return ExecutionResult.Success(user);
        }
        
        private bool IsValidToken(NameValueCollection queryParams)
        {
            var dataHash = queryParams["hash"];

            if (string.IsNullOrWhiteSpace(dataHash))
            {
                return false;
            }

            var dataCheckString = string.Join('\n', queryParams.AllKeys
                .OrderBy(key => key)
                .Where(key => key != null && key != "hash")
                .Select(key => $"{key}={queryParams[key]}"));

            var secretKey = HMACSHA256.HashData(
                Encoding.UTF8.GetBytes("WebAppData"),
                Encoding.UTF8.GetBytes(_configuration.Token));

            var generatedHash = HMACSHA256.HashData(
                secretKey,
                Encoding.UTF8.GetBytes(dataCheckString));

            var actualHash = Convert.FromHexString(dataHash);

            return actualHash.SequenceEqual(generatedHash);
        }

        private TelegramAppUser? CreateUser(string? json)
        {
            return json is null 
                ? null 
                : JsonSerializer.Deserialize<TelegramAppUser>(json);
        }
    }
}
