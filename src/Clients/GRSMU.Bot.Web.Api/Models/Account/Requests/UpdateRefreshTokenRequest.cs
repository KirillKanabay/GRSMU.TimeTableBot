namespace GRSMU.Bot.Web.Api.Models.Account.Requests
{
    public record UpdateRefreshTokenRequest(
        string AccessToken,
        string RefreshToken);
}
