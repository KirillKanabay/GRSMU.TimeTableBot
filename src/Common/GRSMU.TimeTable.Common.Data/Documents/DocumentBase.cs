using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GRSMU.TimeTable.Common.Data.Documents
{
    [BsonIgnoreExtraElements]
    public abstract class DocumentBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
