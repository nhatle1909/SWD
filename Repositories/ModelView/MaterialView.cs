using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using static Repository.Model.Material;

namespace Repository.ModelView
{
    public class MaterialView
    {
        public class AddMaterialView
        {
            public required string Jwt { get; set; }
            [StringLength(50, MinimumLength = 1)]
            public required string MaterialName { get; set; }
            public required ClassifyMaterial MaterialType { get; set; }
            [Range(0, double.MaxValue)]
            public required double Price { get; set; }
        }

        public class UpdateMaterialView
        {
            public required string Jwt { get; set; }
            public required string MaterialId { get; set; }

            [StringLength(50, MinimumLength = 1)]
            public required string MaterialName { get; set; }
            public required ClassifyMaterial MaterialType { get; set; }

            [Range(0, double.MaxValue)]
            public required double Price { get; set; }
        }

        public class DeleteMaterialView
        {
            public required string Jwt { get; set; }
            public required string MaterialId { get; set; }
        }
    }
}
