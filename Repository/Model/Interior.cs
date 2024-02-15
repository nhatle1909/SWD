using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Repository.Model
{
    public class Interior
    {
        [BsonId][BsonRepresentation(BsonType.String)] public required string InteriorId { get; set; }
        [BsonElement] public required string InteriorName { get; set; }
        [BsonElement] public required string Size { get; set; }
        [BsonElement][BsonRepresentation(BsonType.String)] public required string MaterialId { get; set; }
        [BsonElement] public required string Description { get; set; }
        [BsonElement] public required string UrlImage { get; set; }
        [BsonElement] public required int Quantity { get; set; }
        [BsonElement] public required int Price { get; set; }

    }
}
