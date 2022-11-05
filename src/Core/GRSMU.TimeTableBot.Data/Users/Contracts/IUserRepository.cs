using GRSMU.TimeTableBot.Data.Users.Documents;

namespace GRSMU.TimeTableBot.Data.Users.Contracts;

public interface IUserRepository
{
    public Task<UserDocument> GetByTelegramId(string telegramId);

    Task InsertAsync(UserDocument document);

    Task UpdateOneAsync(UserDocument document);
}