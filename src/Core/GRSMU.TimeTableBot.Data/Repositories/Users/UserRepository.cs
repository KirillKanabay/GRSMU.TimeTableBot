using GRSMU.TimeTable.Common.Data.Contexts;
using GRSMU.TimeTable.Common.Data.Immutable;
using GRSMU.TimeTable.Common.Data.Repositories;
using GRSMU.TimeTableBot.Data.Documents;
using MongoDB.Driver.Linq;

namespace GRSMU.TimeTableBot.Data.Repositories.Users
{
    public class UserRepository : RepositoryBase<UserDocument>, IUserRepository
    {
        protected override string CollectionName => CollectionNames.User;

        public UserRepository(IDbContext dbContext) : base(dbContext)
        {
        }


        public Task<UserDocument> GetByTelegramId(string telegramId)
        {
            return GetQuery().Where(x => x.TelegramId.Equals(telegramId)).FirstOrDefaultAsync();
        }
    }
}
