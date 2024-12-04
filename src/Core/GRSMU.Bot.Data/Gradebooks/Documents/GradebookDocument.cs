using GRSMU.Bot.Common.Data.Documents;
using MongoDB.Bson;

namespace GRSMU.Bot.Data.Gradebooks.Documents
{
    public class GradebookDocument : DocumentBase
    {
        public ObjectId UserId { get; set; }

        public string Discipline { get; set; }

        public List<MarkDocument> Marks { get; set; }

        public string CurrentAverageMark { get; set; }

        public string TotalAverageMark { get; set; }

        public string ExamMark { get; set; }
    }
}
