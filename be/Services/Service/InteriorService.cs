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

        public async Task<string> DeleteInterior(DeleteInteriorView delete)
        {
            IEnumerable<Interior> getInterior = await _unit.InteriorRepo.GetFieldsByFilterAsync([],
                       g => g.InteriorId.Equals(delete.InteriorId));
            var interior = getInterior.FirstOrDefault();
            if (interior is not null)
            {
                await _unit.InteriorRepo.RemoveItemByValue("InteriorId", delete.InteriorId);
                return "Delete Interior successfully";
            }
            return "Interior is not existed";
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
            var responses = new List<object>();

            foreach (var item in items)
            {
                responses.Add(new
                {
                    InteriorId = item.InteriorId,
                    InteriorName = item.InteriorName,
                    Image = SomeTool.GetImage(Convert.ToBase64String(item.Image)),
                    Price = item.Price,
                    CreatedAt = item.CreatedAt
                });
            }
            return responses;
        }

        public async Task<Interior?> GetInteriorDetail(string interiorId)
        {
            var getInterior = (await _unit.InteriorRepo.GetFieldsByFilterAsync([],
                            g => g.InteriorId.Equals(interiorId))).FirstOrDefault();
            if (getInterior is not null)
            {
                getInterior.Image = SomeTool.GetImage(Convert.ToBase64String(getInterior.Image))!;
                return getInterior;
            }
            return null;
        }

        public async Task<string> UpdateInterior(UpdateInteriorView update)
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
                    interior.UpdatedAt = System.DateTime.UtcNow;
                    await _unit.InteriorRepo.UpdateItemByValue("InteriorId", update.InteriorId, interior);
                    return "Update interior successfully";
                }
                return "Missing the Image";
            }
            return "Interior is not existed";
        }
    }
}
