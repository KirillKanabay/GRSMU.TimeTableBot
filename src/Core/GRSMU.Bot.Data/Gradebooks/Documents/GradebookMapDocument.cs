using GRSMU.Bot.Common.Data.Mongo.Documents;

namespace GRSMU.Bot.Data.Gradebooks.Documents;

public class GradebookMapDocument : DocumentBase
{
    public string GradebookName { get; set; }
}