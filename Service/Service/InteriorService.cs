using AutoMapper;
using MongoDB.Bson;
using Repository.Model;
using Repository.ModelView;
using Repository.Repository;
using Service.Interface;

namespace Service.Service
{
    public class InteriorService : IInteriorService
    {
        private readonly IRepository<Interior> _InteriorRepos;
        private readonly IRepository<Material> _MaterialRepos;
        private readonly IMapper _Mapper;
        public InteriorService(IRepository<Interior> interiorRepos, IRepository<Material> materialRepos, IMapper mapper)
        {
            _InteriorRepos = interiorRepos;
            _MaterialRepos = materialRepos;
            _Mapper = mapper;
        }

        public async Task<Interior> AddOneInterior(InteriorView interiorView)
        {

            IEnumerable<Material> checkMaterial = await _MaterialRepos.GetByFilterAsync(a => a.MaterialId.Equals(interiorView.MaterialId));
            if (!checkMaterial.Any())
            {
                throw new Exception("Material does not exist");
            }
            Interior item = _Mapper.Map<Interior>(interiorView);
            item.InteriorId = ObjectId.GenerateNewId().ToString();

            return await _InteriorRepos.AddOneItem(item);
        }

        public async Task<bool> DeleteInterior(string id)
        {
            IEnumerable<Interior> item = await _InteriorRepos.GetByFilterAsync(a => a.InteriorId.Equals(id));
            if (!item.Any())
            {
                throw new Exception("Interior does not exist");
            }
            return await _InteriorRepos.RemoveItemByValue(id);
        }

        public async Task<IEnumerable<Interior>> GetAllInterior()
        {
            return await _InteriorRepos.GetAllAsync();
        }

        public async Task<object> GetPagedInterior(int pageIndex, int pageSize, bool isAsc, string sortField, string searchValue, string searchField)
        {
            int skip = (pageIndex - 1) * pageSize;
            var item = await _InteriorRepos.GetPagedAsync(skip, pageSize, isAsc, sortField, searchValue, searchField);
            long totalCount = await _InteriorRepos.CountAsync();
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

        public async Task<Interior> UpdateInterior(string id, InteriorView interiorView)
        {
            IEnumerable<Interior> item = await _InteriorRepos.GetByFilterAsync(a => a.InteriorId.Equals(id));
            if (!item.Any())
            {
                throw new Exception("Interior does not exist");
            }
            IEnumerable<Material> checkMaterial = await _MaterialRepos.GetByFilterAsync(a => a.MaterialId.Equals(interiorView.MaterialId));
            if (!checkMaterial.Any())
            {
                throw new Exception("Material does not exist");
            }
            Interior newItem = _Mapper.Map<Interior>(item);
            newItem.InteriorId = id;
            return await _InteriorRepos.UpdateItemByValue(id, newItem);
        }
    }
}
