
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
            string _id = AuthenticationJwtTool.GetUserIdFromJwt(add.Jwt);
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
            string _id = AuthenticationJwtTool.GetUserIdFromJwt(delete.Jwt);
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

        public async Task<object> GetPagingMaterial(PagingMaterialView paging)
        {
            const int pageSize = 5;
            const string sortField = "CreatedAt";
            List<string> searchFields = ["MaterialName", "Price"];
            List<string> returnFields = [];

            string _id = AuthenticationJwtTool.GetUserIdFromJwt(paging.Jwt);
            var getUserStatus = (await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["IsRole"],
                            g => g.AccountId.Equals(_id))).FirstOrDefault();
            if (getUserStatus != null)
            {
                if (getUserStatus.IsRole == AccountStatus.Role.Staff)
                {
                    int skip = (paging.PageIndex - 1) * pageSize;
                    var items = (await _unit.MaterialRepo.PagingAsync(skip, pageSize, paging.IsAsc, sortField, paging.SearchValue, searchFields, returnFields)).ToList();
                    return items;
                }
                return "You have not permission to use this function";
            }
            return "Account is not existed";
        }

        public async Task<string> UpdateMaterial(UpdateMaterialView update)
        {
            string _id = AuthenticationJwtTool.GetUserIdFromJwt(update.Jwt);
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
                    material.UpdatedAt = System.DateTime.Now;
                    await _unit.MaterialRepo.UpdateItemByValue("MaterialId", update.MaterialId, material);
                    return "Update material successfully";
                }
                return "Material is not existed";
            }
            return "Account is not existed or You have not permission to use this function";
        }
    }
}
