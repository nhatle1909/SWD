using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Models;

namespace Repositories.Model
{
    public class RefreshToken
    {
        [BsonId][BsonRepresentation(BsonType.String)]
        public required string RefreshTokenId { get; set; } 
        [BsonElement][BsonRepresentation(BsonType.String)]
        public required string AccountId { get; set; }
        [BsonElement] public required string Refresh_Token { get; set; }
        [BsonElement] public required string JwtId { get; set; }
        [BsonElement] public bool IsUsed { get; set; }
        [BsonElement] public bool IsRevoked { get; set; }
        [BsonElement] public DateTime IssuedAt { get; set; }
        [BsonElement] public DateTime ExpiredAt { get; set; }
    }
}
