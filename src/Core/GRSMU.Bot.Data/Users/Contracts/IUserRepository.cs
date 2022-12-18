using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Data.Users.Contracts.Filters;
using GRSMU.Bot.Data.Users.Documents;

namespace GRSMU.Bot.Data.Users.Contracts;

public interface IUserRepository
{
    public Task<UserDocument> GetByTelegramIdAsync(string telegramId);

    Task InsertAsync(UserDocument document);

    Task UpdateOneAsync(UserDocument document);

    Task<List<UserDocument>> GetUserListAsync(UserFilter filter, PagingModel paging);
}