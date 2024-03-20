using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Model
{
    public class Contract
    {
        [BsonId][BsonRepresentation(BsonType.String)]
        public required string ContractId { get; set; }
        [BsonElement][BsonRepresentation(BsonType.String)]
        public required string RequestId { get; set; }
        [BsonElement] public required string EmailOfCustomer { get; set; }
        [BsonElement] public required DateTime CreatedAt { get; set; }
        [BsonElement] public required string Description { get; set; }
        [BsonElement] public required byte[] ContractFile { get; set; }
    }
}
