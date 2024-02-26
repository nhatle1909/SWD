using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Models;

namespace Repositories.Model
{
    public class Blog : DateAndTime
    {
        [BsonId][BsonRepresentation(BsonType.ObjectId)] public required string BlogId { get; set; }
        [BsonElement][EmailAddress] public required string Email { get; set; }
        [BsonElement] public required string Title { get; set; }
        [BsonElement] public required string Content { get; set; }
        [BsonElement] public required List<byte[]> Pictures { get; set; }
    }
}
