using GRSMU.TimeTableBot.Common.Data.Documents;

namespace GRSMU.TimeTable.Common.Data.Documents
{
    public class MigrationDocument : DocumentBase
    {
        public string Name { get; set; }

        public string Version { get; set; }
    }
}
