using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using static Repository.Model.Account;

namespace Repository.Models
{
    public class AccountStatus : BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] public required string _id { get; set; }
        [BsonElement] public required string Email { get; set; }
        [BsonElement] public required bool IsAuthenticationEmail { get; set; } = false;
        [BsonElement] public required Role IsRole { get; set; }
        [BsonElement] public required bool IsBanned { get; set; } = false;
        [BsonElement] public string? Comments { get; set; }
    }
}
