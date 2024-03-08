using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Model
{
    public class Cart 
    {
        [BsonId][BsonRepresentation(BsonType.String)]
        public required string CartId { get; set; } = ObjectId.GenerateNewId().ToString();
        [BsonElement][BsonRepresentation(BsonType.String)]
        public required string AccountId { get; set; }      
        [BsonElement][BsonRepresentation(BsonType.String)]
        public required string InteriorId { get; set; }
        [BsonElement] public required int Quantity { get; set; }
    }
}
