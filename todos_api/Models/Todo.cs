using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyApiProject.Models
{
    public class Todo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Title")]
        public string Title { get; set; }

        [BsonElement("IsCompleted")]
        public bool IsCompleted { get; set; }
    }
}
