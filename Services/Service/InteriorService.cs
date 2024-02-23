using AutoMapper;
using MongoDB.Bson;
using Repository.Model;
using Repository.Models;
using Repository.ModelView;
using Repository.Repository;
using Repository.Tools;
using Service.Interface;
using System.Linq;
using static Repository.ModelView.InteriorView;

namespace Service.Service
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

            string _id = Authentication.GetUserIdFromJwt(add.Jwt);
            IEnumerable<AccountStatus> getUser = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["_id", "IsRole"],
                            c => c.AccountId.Equals(_id));
            var accountStatus = getUser.FirstOrDefault();
            if (accountStatus is not null && accountStatus.IsRole == AccountStatus.Role.Staff)
            {
                if (add.Image.Length > 0)
                {
                    var interiorID = ObjectId.GenerateNewId().ToString();
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "InteriorPictures", $"{interiorID}_" + add.Image.FileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        await add.Image.CopyToAsync(stream);
                    }
                    Interior interior = _mapper.Map<Interior>(add);
                    interior.InteriorId = interiorID;
                    interior.Image = "InteriorPictures" + $"{interiorID}_" + add.Image.FileName;
                    await _unit.InteriorRepo.AddOneItem(interior);
                    return "Add Interior successfully";
                }
                return "Missing the Image";
            }
            return "Account is not existed or You have not permission to use this function";
        }

        public async Task<string> DeleteInterior(DeleteInteriorView delete)
        {
            string _id = Authentication.GetUserIdFromJwt(delete.Jwt);
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

        public async Task<IEnumerable<Interior>> GetAllInterior()
        {
            return await _unit.InteriorRepo.GetAllAsync();
        }

        public async Task<object> GetPagedInterior(int pageIndex, int pageSize, bool isAsc, string sortField, string searchValue, string searchField)
        {
            int skip = (pageIndex - 1) * pageSize;
            var item = await _unit.InteriorRepo.GetPagedAsync(skip, pageSize, isAsc, sortField, searchValue, searchField);
            long totalCount = await _unit.InteriorRepo.CountAsync();
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
            string _id = Authentication.GetUserIdFromJwt(update.Jwt);
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
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "InteriorPictures", $"{interior.InteriorId}_" + update.Image.FileName);
                        using (var stream = System.IO.File.Create(path))
                        {
                            await update.Image.CopyToAsync(stream);
                        }
                        interior.InteriorName = update.InteriorName;
                        interior.MaterialId = update.MaterialId;
                        interior.Size = update.Size;
                        interior.InteriorType = update.InteriorType;
                        interior.Description = update.Description;
                        interior.Image = "InteriorPictures" + $"{interior.InteriorId}_" + update.Image.FileName;
                        interior.Quantity = update.Quantity;
                        interior.Price = update.Price;
                        interior.UpdatedAt = DateTime.Now;
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
