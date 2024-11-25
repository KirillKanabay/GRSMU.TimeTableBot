using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Logic.Features.Users.Dtos;
using GRSMU.Bot.Web.Core.Authorization.Models;

namespace GRSMU.Bot.Web.Core.Authorization.Services.Interfaces;

public interface IAccountService
{
    public Task<TokenModel> LoginAsync(UserDto user);

    public Task<ExecutionResult<TokenModel>> RefreshAsync(string accessToken, string refreshToken);
}