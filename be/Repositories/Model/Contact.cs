using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Repositories.Models;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Model
{
    public class Contact : DateAndTime
    {
        [BsonId][BsonRepresentation(BsonType.String)] 
        public required string ContactId { get; set; } = ObjectId.GenerateNewId().ToString();
        [BsonElement] public required string Email { get; set; }
        [BsonElement] public required string Title { get; set; }
        [BsonElement] public required string Content { get; set; }
        [BsonElement] public string? ResponseOfStaff { get; set; }
        [BsonElement] public List<byte[]>? Pictures { get; set; }
        [BsonElement] public required State Status { get; set; }
        public enum State
        {
            Processing = 1, Completed = 2
        }
    }
}
