namespace GRSMU.Bot.Web.Core.Authorization.Models;

public class TokenModel
{
    public Token AccessToken { get; set; }

    public Token RefreshToken { get; set; }

    public TokenModel(Token accessToken, Token refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}