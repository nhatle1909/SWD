using Microsoft.AspNetCore.Http;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using static Repositories.Model.Interior;

namespace Repositories.ModelView
{
    public class InteriorView
    {
        public class AddInteriorView
        {
            [StringLength(50, MinimumLength = 1)]
            public required string InteriorName { get; set; }
            public required ClassifyInterior InteriorType { get; set; }
            public string? Description { get; set; }
            public required IFormFile Image { get; set; }
            [Range(0, int.MaxValue)]
            public required int Quantity { get; set; }
            [Range(0, double.MaxValue)]
            public required double Price { get; set; }
        }

        public class UpdateInteriorView
        {
            [StringLength(50, MinimumLength = 1)]
            public required string InteriorId { get; set; }
            public required string InteriorName { get; set; }
            public required ClassifyInterior InteriorType { get; set; }
            public string? Description { get; set; }
            public required IFormFile Image { get; set; }
            [Range(1, int.MaxValue)]
            public required int Quantity { get; set; }
            [Range(0, double.MaxValue)]
            public required double Price { get; set; }
        }

        public class DeleteInteriorView
        {
            public required string InteriorId { get; set; }
        }

    }
}
