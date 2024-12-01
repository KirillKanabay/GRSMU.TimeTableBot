namespace GRSMU.Bot.Web.Core.Configurations;

public record TelegramConfiguration
{
    public const string SectionName = "Telegram";

    public required string Token { get; set; }
}