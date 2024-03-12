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
        [BsonElement][BsonRepresentation(BsonType.String)]
        public required List<string> InteriorId { get; set; }
        [BsonElement] public required string Email { get; set; }
        [BsonElement] public required string Phone { get; set; }
        [BsonElement] public required string Address { get; set; }
        [BsonElement] public required string Content { get; set; }
        [BsonElement] public byte[]? Picture { get; set; }
        [BsonElement] public string? ResponseOfStaff{ get; set; }
        [BsonElement] public required StateContact StatusOfContact { get; set; }
        [BsonElement] public State? StatusResponseOfStaff { get; set; }
        public enum State
        {
            Awaiting_Payment = 1, Completed = 2
        }
        public enum StateContact
        {
            Processing = 1, Completed = 2
        }
    }
}
