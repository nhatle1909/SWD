using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Models.Model
{
    public class Account
    {
        [BsonId] [BsonRepresentation(MongoDB.Bson.BsonType.String)]public required string AccountId { get; set; } 
        [BsonElement] public required string Username { get; set; }
        [BsonElement][Phone(ErrorMessage ="Invalid")] public required string Password { get; set; }
        [BsonElement][EmailAddress(ErrorMessage ="Invalid")] public required string Email { get; set; }
        [BsonElement] public required string PhoneNumber { get; set; }
        [BsonElement] public required string Role { get; set; }
        [BsonElement] public required bool IsBanned { get; set; }
    }
}
