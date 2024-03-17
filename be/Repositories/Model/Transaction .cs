using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Repositories.Models;

namespace Repositories.Model
{
    public class Transaction : DateAndTime
    {
        [BsonId][BsonRepresentation(BsonType.String)] public required string TransactionId { get; set; }
        [BsonElement] public required string Email { get; set; }
        [BsonElement] public required string TransactionStatus { get; set; }
        [BsonElement] public required int TotalPrice { get; set; }
        [BsonElement] public required int RemainPrice { get; set; }
        [BsonElement] public required DateTime ExpiredDate { get; set; }
        [BsonElement] public required TransactionDetail[] TransactionDetail { get; set; }


    }
    public class TransactionDetail
    {

        [BsonElement]
        [BsonRepresentation(BsonType.String)]
        public required string InteriorId { get; set; }
        [BsonElement] public required int Quantity { get; set; }
    }
}