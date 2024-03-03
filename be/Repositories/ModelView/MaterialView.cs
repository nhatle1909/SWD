using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using static Repositories.Model.Material;

namespace Repositories.ModelView
{
    public class MaterialView
    {
        public class AddMaterialView
        {
            [StringLength(50, MinimumLength = 1)]
            public required string MaterialName { get; set; }
            public required ClassifyMaterial MaterialType { get; set; }
            [Range(0, double.MaxValue)]
            public required double Price { get; set; }
        }

        public class UpdateMaterialView
        {
            public required string MaterialId { get; set; }

            [StringLength(50, MinimumLength = 1)]
            public required string MaterialName { get; set; }
            public required ClassifyMaterial MaterialType { get; set; }

            [Range(0, double.MaxValue)]
            public required double Price { get; set; }
        }

        public class DeleteMaterialView
        {
            public required string MaterialId { get; set; }
        }

        public class PagingMaterialView
        {
            public int PageIndex { get; set; }
            public bool IsAsc { get; set; }
            public string? SearchValue { get; set; }
        }
    }
}
