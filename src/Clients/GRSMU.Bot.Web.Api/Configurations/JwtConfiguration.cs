namespace GRSMU.Bot.Web.Api.Configurations;

public class JwtConfiguration
{
    public const string SectionName = "Jwt";

    public required string AccessTokenKey { get; init; }
    
    public required int AccessTokenLifespanInMinutes { get; init; }
    
    public required int RefreshTokenLifespanInMinutes { get; init; }
    
    public required string Issuer { get; init; }
    
    public required string Audience { get; init; }
}