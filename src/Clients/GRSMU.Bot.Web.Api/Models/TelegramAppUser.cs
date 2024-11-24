using System.Text.Json.Serialization;

namespace GRSMU.Bot.Web.Api.Models
{
    public record TelegramAppUser
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; init; }

        [JsonPropertyName("last_name")]
        public string LastName { get; init; }

        [JsonPropertyName("username")]
        public string UserName { get; init; }

        [JsonPropertyName("language_code")]
        public string LanguageCode { get; init; }

        [JsonPropertyName("allows_write_to_pm")]
        public bool AllowsWriteToPM { get; init; }

        [JsonPropertyName("photo_url")]
        public string PhotoUrl { get; init; }
    }
}
