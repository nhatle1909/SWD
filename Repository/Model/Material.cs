using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Repository.Models;
using System.ComponentModel.DataAnnotations;

namespace Repository.Model
{
    public class Material : BaseEntity
    {
        [BsonId][BsonRepresentation(BsonType.ObjectId)] public required string MaterialId { get; set; }
        [BsonElement] public required string MaterialName { get; set; }
        [BsonElement] public required ClassifyMaterial MaterialType { get; set; }
        [BsonElement] public required double Price { get; set; }
        public enum ClassifyMaterial
        {
            Structural_Material = 1, Colour = 2
        }
    }
}
