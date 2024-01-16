using MongoDB.Bson.Serialization.Attributes;

namespace Models.Model
{
    public class TemplateModel
    {
        [BsonId] public required string id { get; set; }
        [BsonElement] public string? name { get; set; }
    }
}
