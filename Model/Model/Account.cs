using MongoDB.Bson.Serialization.Attributes;

namespace Models.Model
{
    public class Account
    {
        [BsonId] public required string AccountId { get; set; }
        [BsonElement] public required string Username { get; set; }
        [BsonElement] public required string Password { get; set; }
        [BsonElement] public required string Email { get; set; }
        [BsonElement] public required string PhoneNumber { get; set; }
        [BsonElement] public required string Role { get; set; }
        [BsonElement] public required bool IsBanned { get; set; }
    }
}
