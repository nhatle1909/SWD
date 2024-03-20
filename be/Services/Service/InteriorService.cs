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

        public async Task<(bool, string)> AddOneInterior(AddInteriorView add)
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
                return (true, "Add Interior successfully");
            }
            return (false, "Missing the Image");
        }

        public async Task<(bool, string)> DeleteInterior(DeleteInteriorView delete)
        {
            IEnumerable<Interior> getInterior = await _unit.InteriorRepo.GetFieldsByFilterAsync([],
                       g => g.InteriorId.Equals(delete.InteriorId));
            var interior = getInterior.FirstOrDefault();
            if (interior is not null)
            {
                await _unit.InteriorRepo.RemoveItemByValue("InteriorId", delete.InteriorId);
                return (true, "Delete Interior successfully");
            }
            return (false, "Interior is not existed");
        }

        public async Task<object> GetPagingInterior(int pageIndex, bool isAsc, string? searchValue)
        {
            const int pageSize = 100;
            const string sortField = "CreatedAt";
            List<string> searchFields = ["InteriorName", "Description", "Price"];
            List<string> returnFields = [];

            int skip = (pageIndex - 1) * pageSize;
            var items = (await _unit.InteriorRepo.PagingAsync(skip, pageSize, isAsc, sortField, searchValue, searchFields, returnFields)).ToList();
            //long totalCount = await _unit.InteriorRepo.CountAsync();
            //int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var responses = new List<object>();

            return items;
        }

        public async Task<(bool, object)> GetInteriorDetail(string interiorId)
        {
            var getInterior = (await _unit.InteriorRepo.GetFieldsByFilterAsync([],
                            g => g.InteriorId.Equals(interiorId))).FirstOrDefault();
            if (getInterior is not null)
            {
                getInterior.Image = SomeTool.GetImage(Convert.ToBase64String(getInterior.Image))!;
                return (true, getInterior);
            }
            return (false, "Interior is not existed");
        }

        public async Task<(bool, string)> UpdateInterior(UpdateInteriorView update)
        {
            IEnumerable<Interior> getInterior = await _unit.InteriorRepo.GetFieldsByFilterAsync([],
                       g => g.InteriorId.Equals(update.InteriorId));
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
                    interior.InteriorType = update.InteriorType;
                    interior.Description = update.Description;
                    interior.Image = fileBytes;
                    interior.Quantity = update.Quantity;
                    interior.Price = update.Price;
                    interior.UpdatedAt = System.DateTime.Now;
                    await _unit.InteriorRepo.UpdateItemByValue("InteriorId", update.InteriorId, interior);
                    return (true, "Update interior successfully");
                }
                return (false, "Missing the Image");
            }
            return (false, "Interior is not existed");
        }       
    }
}
