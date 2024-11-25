using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Logic.Features.Users.Dtos;
using GRSMU.Bot.Web.Core.Authorization.Models;
using GRSMU.Bot.Web.Core.Authorization.Services.Interfaces;
using GRSMU.Bot.Web.Core.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GRSMU.Bot.Web.Core.Authorization.Services;

public class JwtService : IJwtService
{
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly ILogger<JwtService> _logger;

    public JwtService(IOptions<JwtConfiguration> jwtConfiguration, ILogger<JwtService> logger)
    {
        _logger = logger;
        _jwtConfiguration = jwtConfiguration.Value;
    }

    public Token GenerateAccessToken(UserDto user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.AccessTokenKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(CustomClaimTypes.Id, user.Id),
            new(CustomClaimTypes.StudentCardRegistered, user.IsStudentCardRegistered.ToString()),
        };

        var tokenExpire = DateTime.UtcNow.AddMinutes(_jwtConfiguration.AccessTokenLifespanInMinutes);

        var token = new JwtSecurityToken(
            _jwtConfiguration.Issuer,
            _jwtConfiguration.Audience,
            claims,
            null,
            tokenExpire,
            credentials);

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new Token(jwtToken, tokenExpire);
    }

    public Token GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(randomNumber);
        var refreshToken = Convert.ToBase64String(randomNumber);

        return new Token(refreshToken, DateTime.UtcNow.AddMinutes(_jwtConfiguration.RefreshTokenLifespanInMinutes));
    }

    public ExecutionResult<ClaimsPrincipal?> GetPrincipalFromExpiredToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.AccessTokenKey)),
            ValidateLifetime = false,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || 
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            _logger.LogWarning($"Token is invalid. {accessToken}");
            return ExecutionResult.Failure<ClaimsPrincipal?>(Error.Problem("Authorization.Token", "Token is invalid."));
        }

        return ExecutionResult.Success<ClaimsPrincipal?>(principal);
    }
}