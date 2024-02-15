﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Repository.Model
{
    public class Request
    {
        [BsonId][BsonRepresentation(BsonType.String)] public required string RequiredId { get; set; }
        [BsonElement][BsonRepresentation(BsonType.String)] public required string AccountId { get; set; }

    }
}
