using GRSMU.TimeTableBot.Common.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GRSMU.TimeTable.Common.Data.Extensions
{
    public static class MongoQueryableExtensions
    {
        private const int DefaultPageSize = 10;
        private const int DefaultPage = 1;

        public static async Task<List<TDocument>> ToPagedListAsync<TDocument>(this IMongoQueryable<TDocument> query, PagingModel paging, Dictionary<string, Func<object>> sortingFields = null)
        {
            paging.TotalCount = await query.CountAsync();

            if (sortingFields != null && !string.IsNullOrWhiteSpace(paging.SortBy) && sortingFields.ContainsKey(paging.SortBy))
            {
                query = query.OrderBy(x => sortingFields[paging.SortBy]);
            }

            if (paging.Page <= 0)
            {
                paging.Page = DefaultPage;
            }

            if (paging.PageSize <= 0)
            {
                paging.PageSize = DefaultPageSize;
            }

            query = query.Skip((paging.Page - 1) * paging.PageSize).Take(paging.PageSize);

            return await query.ToListAsync();
        }
    }
}
