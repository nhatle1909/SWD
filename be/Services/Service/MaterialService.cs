
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Repositories.Model;
using Repositories.Models;
using Repositories.ModelView;
using Repositories.Repository;
using Services.Tool;
using Services.Interface;
using static Repositories.ModelView.MaterialView;

namespace Services.Service
{
    public class MaterialService : IMaterialService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly ILogger<MaterialService> _logger;
        public MaterialService(IUnitOfWork unit, IMapper mapper, ILogger<MaterialService> logger)
        {
            _unit = unit;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<string> AddOneMaterial(AddMaterialView add)
        {
            Material material = _mapper.Map<Material>(add);
            await _unit.MaterialRepo.AddOneItem(material);
            return "Add material successfully";
        }

        public async Task<string> DeleteMaterial(DeleteMaterialView delete)
        {
            var getMaterial = await _unit.MaterialRepo.GetFieldsByFilterAsync([],
                       g => g.MaterialId.Equals(delete.MaterialId));
            var material = getMaterial.FirstOrDefault();
            if (material is not null)
            {
                await _unit.MaterialRepo.RemoveItemByValue("MaterialId", delete.MaterialId);
                return "Delete material successfully";
            }
            return "Material is not existed";
        }

        public async Task<object> GetPagingMaterial(PagingMaterialView paging)
        {
            const int pageSize = 5;
            const string sortField = "CreatedAt";
            List<string> searchFields = ["MaterialName", "Price"];
            List<string> returnFields = [];

            int skip = (paging.PageIndex - 1) * pageSize;
            var items = (await _unit.MaterialRepo.PagingAsync(skip, pageSize, paging.IsAsc, sortField, paging.SearchValue, searchFields, returnFields)).ToList();
            return items;
        }

        public async Task<string> UpdateMaterial(UpdateMaterialView update)
        {
            var getMaterial = await _unit.MaterialRepo.GetFieldsByFilterAsync([],
                        g => g.MaterialId.Equals(update.MaterialId));
            var material = getMaterial.FirstOrDefault();
            if (material is not null)
            {
                material.MaterialName = update.MaterialName;
                material.MaterialType = update.MaterialType;
                material.Price = update.Price;
                material.UpdatedAt = System.DateTime.UtcNow;
                await _unit.MaterialRepo.UpdateItemByValue("MaterialId", update.MaterialId, material);
                return "Update material successfully";
            }
            return "Material is not existed";
        }

        public async Task<double> OptionalProductQuote(string[] arrMaterialId)
        {
            var materials = await _unit.MaterialRepo.GetFieldsByFilterAsync(["Price"],
                            c => arrMaterialId.Contains(c.MaterialId));
            if (materials.Any())
            {
                double totalPrice = 0;
                foreach (var material in materials)
                    totalPrice += material.Price;
                return totalPrice;
            }
            return 0;
        }

    }
}
