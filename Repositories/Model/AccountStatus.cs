using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using static Repository.Model.Account;

namespace Repository.Models
{
    public class AccountStatus : BaseEntity
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
