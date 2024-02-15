using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Repository.Model
{
    public class Account
    {
        [BsonId][BsonRepresentation(BsonType.String)] public required string AccountId { get; set; }
        [BsonElement] public required string Password { get; set; }
        [BsonElement][EmailAddress(ErrorMessage = "Invalid Mail")] public required string Email { get; set; }
        [BsonElement][Phone(ErrorMessage = "Invalid Phone Number")] public required string PhoneNumber { get; set; }
        [BsonElement] public required string Address { get; set; }
        public enum Role
        {
            Guest = 1, Customer = 2, Staff = 3, Admin = 4
        }
    }
}
