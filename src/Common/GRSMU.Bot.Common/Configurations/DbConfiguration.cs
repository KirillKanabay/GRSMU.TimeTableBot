namespace GRSMU.Bot.Common.Configurations;

public record DbConfiguration
{
    public const string SectionName = "MongoDb";

    public string ConnectionString { get; init; }

    public string DatabaseName { get; init; }
}