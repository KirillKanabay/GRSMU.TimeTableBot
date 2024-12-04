using GRSMU.Bot.Common.Data.Contexts;
using GRSMU.Bot.Common.Data.Immutable;
using GRSMU.Bot.Common.Data.Migrator;
using GRSMU.Bot.Data.Gradebooks.Documents;
using MongoDB.Driver;

namespace GRSMU.Bot.Data.Migrations.Gradebooks
{
    internal class V2_0_3_GradebooksDrop : IMigration
    {
        private const string CollectionName = CollectionNames.Gradebook;

        public Version Version => new Version(2, 0, 3);
        public string Name => $"{CollectionName} - drop";
        
        public Task RunAsync(IDbContext context)
        {
            return context.GetCollection<GradebookDocument>(CollectionName)
                .DeleteManyAsync(FilterDefinition<GradebookDocument>.Empty);
        }
    }
}
