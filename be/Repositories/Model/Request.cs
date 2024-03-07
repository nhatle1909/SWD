using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Repositories.Models;

namespace Repositories.Model
{
    public class Request : DateAndTime
    {
        [BsonId][BsonRepresentation(BsonType.ObjectId)] public required string RequestId { get; set; }
        [BsonElement][BsonRepresentation(BsonType.ObjectId)] public required string AccountId { get; set; }
        [BsonElement] public required string RequestStatus { get; set; }
        [BsonElement] public required int TotalPrice { get; set; }
        [BsonElement] public required RequestDetail[] RequestDetail { get; set; }
        [BsonElement] public required int RemainPrice { get; set; } 
        
    }
    public class RequestDetail 
    {
        public RequestDetail()
        {
           
        }

        [BsonElement]
        [BsonRepresentation(BsonType.String)]
        public required string InteriorId { get; set; }
        [BsonElement] public required int Quantity { get; set; }
    }
}
