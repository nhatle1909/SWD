using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Repository.Models
{
    public class BaseEntity
    {
        [BsonElement] public required DateTime CreatedAt { get; set; } = DateTime.Now;
        [BsonElement] public DateTime? UpdatedAt { get; set; }
    }

}
