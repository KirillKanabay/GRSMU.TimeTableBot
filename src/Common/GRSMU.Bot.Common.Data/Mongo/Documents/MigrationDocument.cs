namespace GRSMU.Bot.Common.Data.Mongo.Documents
{
    public class MigrationDocument : DocumentBase
    {
        public string Name { get; set; }

        public string Version { get; set; }
    }
}
