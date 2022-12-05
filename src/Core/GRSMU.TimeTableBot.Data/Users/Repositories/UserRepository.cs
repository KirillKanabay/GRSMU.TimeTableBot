using System.Text.RegularExpressions;
using GRSMU.TimeTable.Common.Data.Contexts;
using GRSMU.TimeTable.Common.Data.Extensions;
using GRSMU.TimeTable.Common.Data.Immutable;
using GRSMU.TimeTableBot.Common.Data.Repositories;
using GRSMU.TimeTableBot.Common.Models;
using GRSMU.TimeTableBot.Data.Users.Contracts;
using GRSMU.TimeTableBot.Data.Users.Contracts.Filters;
using GRSMU.TimeTableBot.Data.Users.Documents;
using MongoDB.Driver.Linq;

namespace GRSMU.TimeTableBot.Data.Users.Repositories
{
    public class UserRepository : RepositoryBase<UserDocument>, IUserRepository
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
