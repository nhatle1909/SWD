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
        [BsonElement] public required string EmailOfCustomer { get; set; }
        [BsonElement][BsonRepresentation(BsonType.String)]
        public required string StaffId { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required string Description { get; set; }
        public required byte[] ContractFile { get; set; }
        public required State Status { get; set; }
        public enum State
        {
            Pending = 1, Completed = 2, Cancelled = 3
        }
    }
}
