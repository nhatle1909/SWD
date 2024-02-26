using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Model
{
    public class Account
    {
        [BsonId][BsonRepresentation(BsonType.ObjectId)] public required string AccountId { get; set; }
        [BsonElement] public required string Password { get; set; }
        [BsonElement][EmailAddress] public required string Email { get; set; }
        [BsonElement][Phone] public string? PhoneNumber { get; set; }
        [BsonElement] public string? Address { get; set; }
        [BsonElement] public required byte[] Picture { get; set; }
    }
}
