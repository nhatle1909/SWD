using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Repository.Model
{
    public class Material
    {
        [BsonId][BsonRepresentation(MongoDB.Bson.BsonType.String)] public required string MaterialId { get; set; }
        [BsonElement] public required string MaterialName { get; set; }
    }
}
