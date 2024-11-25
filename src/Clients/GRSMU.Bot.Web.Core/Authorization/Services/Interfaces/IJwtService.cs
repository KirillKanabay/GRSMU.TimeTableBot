using System.Security.Claims;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Logic.Features.Users.Dtos;
using GRSMU.Bot.Web.Core.Authorization.Models;

namespace GRSMU.Bot.Web.Core.Authorization.Services.Interfaces;

public interface IJwtService
{
    Token GenerateAccessToken(UserDto user);

    Token GenerateRefreshToken();

    ExecutionResult<ClaimsPrincipal?> GetPrincipalFromExpiredToken(string accessToken);
}