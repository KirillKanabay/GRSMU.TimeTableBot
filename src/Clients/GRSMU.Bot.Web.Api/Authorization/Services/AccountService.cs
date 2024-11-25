using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Logic.Features.Users.Commands.UpdateRefreshToken;
using GRSMU.Bot.Logic.Features.Users.Dtos;
using GRSMU.Bot.Logic.Features.Users.Queries.GetById;
using GRSMU.Bot.Web.Api.Authorization.Models;
using GRSMU.Bot.Web.Api.Authorization.Services.Interfaces;
using MediatR;

namespace GRSMU.Bot.Web.Api.Authorization.Services;

public class AccountService : IAccountService
{
    private readonly IJwtService _jwtService;
    private readonly ISender _sender;
    private readonly ILogger<AccountService> _logger;

    public AccountService(
        IJwtService jwtService,
        ISender sender,
        ILogger<AccountService> logger)
    {
        _jwtService = jwtService;
        _sender = sender;
        _logger = logger;
    }

    public async Task<TokenModel> LoginAsync(UserDto user)
    {
        var refreshToken = _jwtService.GenerateRefreshToken();
        await UpdateRefreshToken(user.Id, refreshToken);

        var accessToken = _jwtService.GenerateAccessToken(user);

        return new TokenModel(accessToken, refreshToken);
    }

    public async Task<ExecutionResult<TokenModel>> RefreshAsync(string accessToken, string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
        {
            _logger.LogWarning("Access or refresh token is empty");
            return ExecutionResult<TokenModel>.Failure(Error.Problem("Authorization.Token", "Access or refresh token is empty"));
        }

        var principalResult = _jwtService.GetPrincipalFromExpiredToken(accessToken);

        if (principalResult.HasErrors)
        {
            return ExecutionResult.Failure<TokenModel>(principalResult.Error!);
        }

        var principal = principalResult.Data;

        if (principal is null)
        {
            _logger.LogWarning($"Can't parse access token while refresh. Access token: {accessToken}");
            return ExecutionResult.Failure<TokenModel>(Error.Problem("Authorization.Token", "Can't parse access token while refresh"));
        }

        var id = principal.GetId();

        var userSearchResult = await _sender.Send(new GetUserByIdQuery(id));

        if (userSearchResult.HasErrors)
        {
            return ExecutionResult.Failure<TokenModel>(userSearchResult.Error!);
        }

        var user = userSearchResult.Data;

        if (user.RefreshToken != refreshToken || user.RefreshTokenExpireTime <= DateTime.UtcNow)
        {
            _logger.LogWarning($"Refresh token is invalid or expired.");

            return ExecutionResult.Failure<TokenModel>(Error.Problem("Authorization.Token", "Refresh token is invalid or expired"));
        }

        var newRefreshToken = _jwtService.GenerateRefreshToken();
        await UpdateRefreshToken(id, newRefreshToken);
        
        var newAccessToken = _jwtService.GenerateAccessToken(user);

        return ExecutionResult.Success(new TokenModel(newAccessToken, newRefreshToken));
    }

    private Task UpdateRefreshToken(string userId, Token refreshToken)
    {
        return _sender.Send(new UpdateRefreshTokenCommand(userId, refreshToken.Value, refreshToken.ExpireTime));
    }
}