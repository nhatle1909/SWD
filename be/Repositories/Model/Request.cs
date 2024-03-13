using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Repositories.Models;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Model
{
    public class Request : DateAndTime
    {
        [BsonId][BsonRepresentation(BsonType.String)] 
        public required string RequestId { get; set; } = ObjectId.GenerateNewId().ToString();
        [BsonElement][BsonRepresentation(BsonType.String)]
        public required List<string> InteriorId { get; set; }
        [BsonElement] public required string Email { get; set; }
        [BsonElement] public required string Phone { get; set; }
        [BsonElement] public required string Address { get; set; }
        [BsonElement] public required string Content { get; set; }
        [BsonElement] public required byte[] Picture { get; set; }
        [BsonElement] public string? ResponseOfStaff{ get; set; }

        [BsonElement] public State? StatusResponseOfStaff { get; set; }
        public enum State
        {
            Awaiting_Payment = 1, Completed = 2
        }

    }
}
