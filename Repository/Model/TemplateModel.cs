using MongoDB.Bson.Serialization.Attributes;

namespace Repository.Model
{
    public class TemplateModel
    {
        [BsonId] public required string id { get; set; }
        [BsonElement] public string? name { get; set; }
    }
}
