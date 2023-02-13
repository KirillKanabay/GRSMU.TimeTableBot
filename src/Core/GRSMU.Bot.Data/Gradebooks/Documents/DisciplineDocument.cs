using MongoDB.Bson;

namespace GRSMU.Bot.Data.Gradebooks.Documents;

public class DisciplineDocument
{
    public ObjectId Id { get; set; }

    public string Name { get; set; }

    public List<MarkDocument> Marks { get; set; }

    public string CurrentAverageMark { get; set; }

    public string TotalAverageMark { get; set; }

    public string ExamMark { get; set; }
}