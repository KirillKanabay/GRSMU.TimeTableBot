using System.Security.Claims;

namespace GRSMU.Bot.Web.Api.Authorization;

public static class ClaimsPrincipalExtensions
{
    public static string GetId(this ClaimsPrincipal principal)
    {
        return GetClaimOfType(principal, CustomClaimTypes.Id);
    }

    private static string GetClaimOfType(ClaimsPrincipal principal, string type)
    {
        return principal.Claims.FirstOrDefault(x => x.Type == type)?.Value ?? string.Empty;
    }
}