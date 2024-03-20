using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Repositories.Models;
using System.ComponentModel.DataAnnotations;
using static Repositories.ModelView.CartView;

namespace Repositories.Model
{
    public class Request : DateAndTime
    {
        [BsonId][BsonRepresentation(BsonType.String)] 
        public required string RequestId { get; set; } = ObjectId.GenerateNewId().ToString();
        [BsonElement] public required AddCartView[] ListInterior { get; set; }
        [BsonElement] public required string Email { get; set; }
        [BsonElement] public required string Phone { get; set; }
        [BsonElement] public required string Address { get; set; }
        [BsonElement] public required string Content { get; set; }
        [BsonElement] public string? ResponseOfStaff{ get; set; }
        [BsonElement] public byte[]? ResponseOfStaffInFile { get; set; }
        [BsonElement] public required State StatusResponseOfStaff { get; set; }
        public enum State
        {
            Completed = 1, Processing = 2, Consulting = 3
        }

    }
}
