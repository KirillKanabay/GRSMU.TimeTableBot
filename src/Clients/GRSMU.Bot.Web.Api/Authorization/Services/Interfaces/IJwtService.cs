using System.Security.Claims;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Logic.Features.Users.Dtos;
using GRSMU.Bot.Web.Api.Authorization.Models;

namespace GRSMU.Bot.Web.Api.Authorization.Services.Interfaces;

public interface IJwtService
{
    Token GenerateAccessToken(UserDto user);

    Token GenerateRefreshToken();

    ExecutionResult<ClaimsPrincipal?> GetPrincipalFromExpiredToken(string accessToken);
}