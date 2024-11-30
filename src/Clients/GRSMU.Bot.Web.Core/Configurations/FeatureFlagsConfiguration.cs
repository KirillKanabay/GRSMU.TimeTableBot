namespace GRSMU.Bot.Web.Core.Configurations;

public record FeatureFlagsConfiguration
{
    public const string SectionName = "FeatureFlags";

    public bool DemoStudentId { get; init; }
}