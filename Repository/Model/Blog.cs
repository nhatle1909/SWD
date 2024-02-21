using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Models;

namespace Repository.Model
{
    public class Blog : BaseEntity
    {
        [BsonId][BsonRepresentation(BsonType.ObjectId)] public required string BlogId { get; set; }
        [BsonElement][BsonRepresentation(BsonType.ObjectId)] public required string AccountId { get; set; }
        [BsonElement] public required string Title { get; set; }
        [BsonElement] public required string Content { get; set; }
    }
}
