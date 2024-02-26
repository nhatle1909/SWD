using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Repositories.Models;

namespace Repositories.Model
{
    public class Interior : DateAndTime
    {
        [BsonId][BsonRepresentation(BsonType.ObjectId)] public required string InteriorId { get; set; }
        [BsonElement][BsonRepresentation(BsonType.ObjectId)] public required string[] MaterialId { get; set; }
        [BsonElement] public required string InteriorName { get; set; }
        [BsonElement] public required int[] Size { get; set; }
        [BsonElement] public required ClassifyInterior InteriorType { get; set; }
        [BsonElement] public string? Description { get; set; }
        [BsonElement] public required byte[] Image { get; set; }
        [BsonElement] public required int Quantity { get; set; }
        [BsonElement] public required double Price { get; set; }
        public enum ClassifyInterior
        {
            Chair = 1, desk = 2
        }
    }
}
