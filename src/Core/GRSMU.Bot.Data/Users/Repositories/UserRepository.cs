using System.Text.RegularExpressions;
using GRSMU.Bot.Common.Data.Mongo.Contexts;
using GRSMU.Bot.Common.Data.Mongo.Extensions;
using GRSMU.Bot.Common.Data.Mongo.Immutable;
using GRSMU.Bot.Common.Data.Mongo.Repositories;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Data.Users.Contracts.Filters;
using GRSMU.Bot.Data.Users.Documents;
using MongoDB.Driver.Linq;

namespace GRSMU.Bot.Data.Users.Repositories
{
    public class UserRepository : MongoRepositoryBase<UserDocument>, IUserRepository
    {
        protected override string CollectionName => CollectionNames.User;

        public UserRepository(IDbContext dbContext) : base(dbContext)
        {
        }

        public Task<UserDocument> GetByTelegramIdAsync(string telegramId)
        {
            return GetQuery().Where(x => x.TelegramId.Equals(telegramId)).FirstOrDefaultAsync();
        }

        public Task<List<UserDocument>> GetUserListAsync(UserFilter filter, PagingModel paging)
        {
            return GetQuery(filter).ToPagedListAsync(paging);
        }

        protected IMongoQueryable<UserDocument> GetQuery(UserFilter filter)
        {
            var query = base.GetQuery();

            if (filter.CourseIds?.Any() ?? false)
            {
                query = query.Where(x => filter.CourseIds.Contains(x.CourseId));
            }

            if (filter.GroupIds?.Any() ?? false)
            {
                query = query.Where(x => filter.GroupIds.Contains(x.GroupId));
            }

            if (filter.FacultyIds?.Any() ?? false)
            {
                query = query.Where(x => filter.FacultyIds.Contains(x.CourseId));
            }

            if (!string.IsNullOrWhiteSpace(filter.FirstName))
            {
                var regex = new Regex(Regex.Escape(filter.FirstName), RegexOptions.IgnoreCase);

                query = query.Where(x => regex.IsMatch(x.FirstName));
            }

            if (!string.IsNullOrWhiteSpace(filter.LastName))
            {
                var regex = new Regex(Regex.Escape(filter.LastName), RegexOptions.IgnoreCase);

                query = query.Where(x => regex.IsMatch(x.LastName));
            }

            if (!string.IsNullOrWhiteSpace(filter.Username))
            {
                var regex = new Regex(Regex.Escape(filter.Username), RegexOptions.IgnoreCase);

                query = query.Where(x => regex.IsMatch(x.Username));
            }

            return query;
        }
    }
}
