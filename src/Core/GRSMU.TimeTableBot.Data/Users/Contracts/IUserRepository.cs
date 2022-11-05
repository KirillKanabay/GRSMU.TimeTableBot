using GRSMU.TimeTableBot.Common.Models;
using GRSMU.TimeTableBot.Data.Users.Contracts.Filters;
using GRSMU.TimeTableBot.Data.Users.Documents;

namespace GRSMU.TimeTableBot.Data.Users.Contracts;

public interface IUserRepository
{
    public Task<UserDocument> GetByTelegramIdAsync(string telegramId);

    Task InsertAsync(UserDocument document);

    Task UpdateOneAsync(UserDocument document);

    Task<List<UserDocument>> GetUserListAsync(UserFilter filter, PagingModel paging);
}