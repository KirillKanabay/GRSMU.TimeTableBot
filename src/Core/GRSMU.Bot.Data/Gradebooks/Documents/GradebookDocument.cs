using GRSMU.Bot.Common.Data.Documents;
using MongoDB.Bson;

namespace GRSMU.Bot.Data.Gradebooks.Documents
{
    public class GradebookDocument : DocumentBase
    {
        public ObjectId UserId { get; set; }

        public List<DisciplineDocument> Disciplines { get; set; }
    }
}
