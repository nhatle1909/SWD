using AutoMapper;
using MongoDB.Bson;
using Repositories.Model;
using Repositories.Models;
using Repositories.ModelView;
using Repositories.Repository;
using Services.Tool;
using Services.Interface;
using System.Linq;
using System.Security.Cryptography;
using static Repositories.ModelView.InteriorView;

namespace Services.Service
{
    public class InteriorService : IInteriorService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        public InteriorService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<string> AddOneInterior(AddInteriorView add)
        {

            string _id = AuthenticationJwtTool.GetUserIdFromJwt(add.Jwt);
            IEnumerable<AccountStatus> getUser = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["_id", "IsRole"],
                            c => c.AccountId.Equals(_id));
            var accountStatus = getUser.FirstOrDefault();
            if (accountStatus is not null && accountStatus.IsRole == AccountStatus.Role.Staff)
            {
                if (add.Image.Length > 0)
                {
                    //Encode picture
                    byte[] fileBytes;
                    using (var ms = new MemoryStream())
                    {
                        await add.Image.CopyToAsync(ms);
                        fileBytes = ms.ToArray();
                    }
                    Interior interior = _mapper.Map<Interior>(add);
                    interior.Image = fileBytes;
                    await _unit.InteriorRepo.AddOneItem(interior);
                    return "Add Interior successfully";
                }
                return "Missing the Image";
            }
            return "Account is not existed or You have not permission to use this function";
        }

        public async Task<string> DeleteInterior(DeleteInteriorView delete)
        {
            string _id = AuthenticationJwtTool.GetUserIdFromJwt(delete.Jwt);
            IEnumerable<AccountStatus> getUser = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["_id", "IsRole"],
                            g => g.AccountId.Equals(_id));
            var accountStatus = getUser.FirstOrDefault();
            if (accountStatus is not null && accountStatus.IsRole == AccountStatus.Role.Staff)
            {
                IEnumerable<Interior> getInterior = await _unit.InteriorRepo.GetFieldsByFilterAsync([],
                           g => g.MaterialId.Equals(delete.InteriorId));
                var interior = getInterior.FirstOrDefault();
                if (interior is not null)
                {
                    await _unit.InteriorRepo.RemoveItemByValue("InteriorId", delete.InteriorId);
                    return "Delete material successfully";
                }
                return "Interior is not existed";
            }
            return "Account is not existed or You have not permission to use this function";
        }

        public async Task<object> GetPagingInterior(int pageIndex, bool isAsc, string? searchValue)
        {
            const int pageSize = 5;
            const string sortField = "CreatedAt";
            List<string> searchFields = ["InteriorName", "Description", "Price"];
            List<string> returnFields = ["InteriorName", "Image", "Price", "CreatedAt"];

            int skip = (pageIndex - 1) * pageSize;
            var items = (await _unit.InteriorRepo.PagingAsync(skip, pageSize, isAsc, sortField, searchValue, searchFields, returnFields)).ToList();
            //long totalCount = await _unit.InteriorRepo.CountAsync();
            //int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            return items;
        }

        public async Task<Interior?> GetInteriorDetail(string interiorId)
        {
            var getInterior = (await _unit.InteriorRepo.GetFieldsByFilterAsync([],
                            g => g.InteriorId.Equals(interiorId))).FirstOrDefault();
            return getInterior;
        }

        //public async Task<double> OptionalInteriorQuote(string[] arrMaterialId)
        //{
        //    IEnumerable<Material> materials = await _repoInterior.GetFieldsByFilterAsync(["Price"],
        //                    c => arrMaterialId.Contains(c.MaterialId));
        //    if (materials.Any())
        //    {
        //        double totalPrice = 0;
        //        foreach (var material in materials)
        //            totalPrice += material.Price;
        //        return totalPrice;
        //    }
        //    return 0;
        //}

        public async Task<string> UpdateInterior(UpdateInteriorView update)
        {
            string _id = AuthenticationJwtTool.GetUserIdFromJwt(update.Jwt);
            IEnumerable<AccountStatus> getUser = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["_id", "IsRole"],
                            g => g.AccountId.Equals(_id));
            var accountStatus = getUser.FirstOrDefault();
            if (accountStatus is not null && accountStatus.IsRole == AccountStatus.Role.Staff)
            {
                IEnumerable<Interior> getInterior = await _unit.InteriorRepo.GetFieldsByFilterAsync([],
                           g => g.MaterialId.Equals(update.InteriorId));
                var interior = getInterior.FirstOrDefault();
                if (interior is not null)
                {
                    if (update.Image.Length > 0)
                    {
                        //Encode picture
                        byte[] fileBytes;
                        using (var ms = new MemoryStream())
                        {
                            await update.Image.CopyToAsync(ms);
                            fileBytes = ms.ToArray();
                        }
                        interior.InteriorName = update.InteriorName;
                        interior.MaterialId = update.MaterialId;
                        interior.Size = update.Size;
                        interior.InteriorType = update.InteriorType;
                        interior.Description = update.Description;
                        interior.Image = fileBytes;
                        interior.Quantity = update.Quantity;
                        interior.Price = update.Price;
                        interior.UpdatedAt = System.DateTime.Now;
                        await _unit.InteriorRepo.UpdateItemByValue("InteriorId", update.InteriorId, interior);
                        return "Update interior successfully";
                    }
                    return "Missing the Image";
                }
                return "Interior is not existed";
            }
            return "Account is not existed or You have not permission to use this function";
        }
    }
}
