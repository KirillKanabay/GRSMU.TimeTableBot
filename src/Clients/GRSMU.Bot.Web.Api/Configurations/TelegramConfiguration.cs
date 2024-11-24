namespace GRSMU.Bot.Web.Api.Configurations
{
    public record TelegramConfiguration
    {
        public const string SectionName = "Telegram";

        public required string Token { get; set; }
    }
}
