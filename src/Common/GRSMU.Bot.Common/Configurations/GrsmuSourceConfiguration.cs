namespace GRSMU.Bot.Common.Configurations;

public record GrsmuSourceConfiguration
{
    public static string SectionName = "GrsmuSource";

    public string TimetableUrl { get; init; }

    public string GradebookUrl { get; init; }

    public string CookieUrl { get; init; }
}