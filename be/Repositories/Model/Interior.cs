using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Repositories.Models;

namespace Repositories.Model
{
    public class Interior : DateAndTime
    {
        [BsonId][BsonRepresentation(BsonType.String)] 
        public required string InteriorId { get; set; } = ObjectId.GenerateNewId().ToString();
        [BsonElement] public required string InteriorName { get; set; }
        [BsonElement] public required ClassifyInterior InteriorType { get; set; }
        [BsonElement] public string? Description { get; set; }
        [BsonElement] public required byte[] Image { get; set; }
        [BsonElement] public required int Quantity { get; set; }
        [BsonElement] public required double Price { get; set; }
        public enum ClassifyInterior
        {
            Chair = 1, Desk = 2
        }
    }
}
