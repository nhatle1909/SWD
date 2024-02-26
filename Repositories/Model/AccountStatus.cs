using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using static Repositories.Model.Account;

namespace Repositories.Models
{
    public class AccountStatus : DateAndTime
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] public required string AccountId { get; set; }
        [BsonElement] public required string Email { get; set; }
        [BsonElement] public required bool IsAuthenticationEmail { get; set; } = false;
        [BsonElement] public required Role IsRole { get; set; }
        [BsonElement] public required bool IsBanned { get; set; } = false;
        [BsonElement] public string? Comments { get; set; }
        public enum Role
        {
            Guest = 1, Customer = 2, Staff = 3, Admin = 4
        }

    }
}
