using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Models;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Model
{
    public class BlogComment : DateAndTime
    {
        [BsonId][BsonRepresentation(BsonType.ObjectId)] public required string BlogCommentId { get; set; }
        [BsonElement][BsonRepresentation(BsonType.ObjectId)] public required string BlogId { get; set; }
        [BsonElement][EmailAddress] public required string Email { get; set; }
        [BsonElement] public required string Comment { get; set; }
    }
}
