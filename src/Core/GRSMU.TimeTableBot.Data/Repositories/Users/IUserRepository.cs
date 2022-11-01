using GRSMU.TimeTableBot.Data.Documents;

namespace GRSMU.TimeTableBot.Data.Repositories.Users;

public interface IUserRepository
{
    public Task<UserDocument> GetByTelegramId(string telegramId);

    Task InsertAsync(UserDocument document);

    Task UpdateOneAsync(UserDocument document);
}