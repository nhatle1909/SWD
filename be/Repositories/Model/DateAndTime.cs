using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Repositories.Models
{
    public class DateAndTime
    {
        [BsonElement] public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [BsonElement] public DateTime? UpdatedAt { get; set; }
    }

}
