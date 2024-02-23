
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Repository.Model;
using Repository.Models;
using Repository.ModelView;
using Repository.Repository;
using Repository.Tools;
using Service.Interface;
using static Repository.ModelView.MaterialView;

namespace Service.Service
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
            string _id = Authentication.GetUserIdFromJwt(add.Jwt);
            IEnumerable<AccountStatus> getUser = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["_id", "IsRole"],
                            c => c.AccountId.Equals(_id));
            var accountStatus = getUser.FirstOrDefault();
            if (accountStatus is not null && accountStatus.IsRole == AccountStatus.Role.Staff)
            {
                Material material = _mapper.Map<Material>(add);
                await _unit.MaterialRepo.AddOneItem(material);
                return "Add material successfully";
            }
            return "Account is not existed or You have not permission to use this function";
        }

        public async Task<string> DeleteMaterial(DeleteMaterialView delete)
        {
            string _id = Authentication.GetUserIdFromJwt(delete.Jwt);
            IEnumerable<AccountStatus> getUser = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["_id", "IsRole"],
                            g => g.AccountId.Equals(_id));
            var accountStatus = getUser.FirstOrDefault();
            if (accountStatus is not null && accountStatus.IsRole == AccountStatus.Role.Staff)
            {
                IEnumerable<Material> getMaterial = await _unit.MaterialRepo.GetFieldsByFilterAsync([],
                           g => g.MaterialId.Equals(delete.MaterialId));
                var material = getMaterial.FirstOrDefault();
                if (material is not null)
                {
                    await _unit.MaterialRepo.RemoveItemByValue("MaterialId", delete.MaterialId);
                    return "Delete material successfully";
                }
                return "Material is not existed";
            }
            return "Account is not existed or You have not permission to use this function";
        }

        public async Task<IEnumerable<Material>> GetAllMaterial()
        {
            return await _unit.MaterialRepo.GetAllAsync();
        }

        public async Task<object> GetPagedMaterial(int pageIndex, int pageSize, bool isAsc, string sortField, string searchValue, string searchField)
        {
            int skip = (pageIndex - 1) * pageSize;
            var item = await _unit.MaterialRepo.GetPagedAsync(skip, pageSize, isAsc, sortField, searchValue, searchField);
            long totalCount = await _unit.MaterialRepo.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var response = new
            {
                TotalCount = totalCount,
                Page = pageIndex,
                PageSize = pageSize,
                Material = item.ToList()
            };
            return response;
        }

        public async Task<string> UpdateMaterial(UpdateMaterialView update)
        {
            string _id = Authentication.GetUserIdFromJwt(update.Jwt);
            IEnumerable<AccountStatus> getUser = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["_id", "IsRole"],
                            g => g.AccountId.Equals(_id));
            var accountStatus = getUser.FirstOrDefault();
            if (accountStatus is not null && accountStatus.IsRole == AccountStatus.Role.Staff)
            {
                IEnumerable<Material> getMaterial = await _unit.MaterialRepo.GetFieldsByFilterAsync([],
                            g => g.MaterialId.Equals(update.MaterialId));
                var material = getMaterial.FirstOrDefault();
                if (material is not null)
                {
                    material.MaterialName = update.MaterialName;
                    material.MaterialType = update.MaterialType;
                    material.Price = update.Price;
                    material.UpdatedAt = DateTime.Now;
                    await _unit.MaterialRepo.UpdateItemByValue("MaterialId", update.MaterialId, material);
                    return "Update material successfully";
                }
                return "Material is not existed";
            }
            return "Account is not existed or You have not permission to use this function";
        }
    }
}
