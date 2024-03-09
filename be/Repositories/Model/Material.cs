using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Repositories.Models;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Model
{
    public class Material : DateAndTime
    {
        [BsonId][BsonRepresentation(BsonType.String)] 
        public required string MaterialId { get; set; } = ObjectId.GenerateNewId().ToString();
        [BsonElement] public required string MaterialName { get; set; }
        //[BsonElement] public required ClassifyMaterial MaterialType { get; set; }
        [BsonElement] public required double Price { get; set; }
        //public enum ClassifyMaterial
        //{
        //    Type = 1, Style = 2, Structural_Material = 3, Colour = 4
        //}
    }
}
