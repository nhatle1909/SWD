
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Repository.Model;
using Repository.ModelView;
using Repository.Repository;
using Service.Interface;

namespace Service.Service
{
    public class MaterialService : IMaterialService
    {
        private readonly IRepository<Material> _repos;
        private readonly IMapper _mapper;
        private readonly ILogger<MaterialService> _logger;
        public MaterialService(IRepository<Material> repos, IMapper mapper, ILogger<MaterialService> logger)
        {
            _repos = repos;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Material> AddOneMaterial(MaterialView materialView)
        {
            Material item = _mapper.Map<Material>(materialView);
            item.MaterialId = ObjectId.GenerateNewId().ToString();
            return await _repos.AddOneItem(item);
        }

        public async Task<bool> DeleteMaterial(string id)
        {
            IEnumerable<Material> item = await _repos.GetByFilterAsync(i => i.MaterialId.Equals(id));
            if (!item.Any())
            {
                throw new Exception($"Item with id {id} not found");
            }
            return await _repos.RemoveItemByValue(id);
        }

        public async Task<IEnumerable<Material>> GetAllMaterial()
        {
            return await _repos.GetAllAsync();
        }

        public async Task<object> GetPagedMaterial(int pageIndex, int pageSize, bool isAsc, string sortField, string searchValue, string searchField)
        {
            int skip = (pageIndex - 1) * pageSize;
            var item = await _repos.GetPagedAsync(skip, pageSize, isAsc, sortField, searchValue, searchField);
            long totalCount = await _repos.CountAsync();
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

        public async Task<Material> UpdateMaterial(string id, MaterialView materialView)
        {
            IEnumerable<Material> item = await _repos.GetByFilterAsync(i => i.MaterialId.Equals(id));
            if (!item.Any())
            {
                throw new Exception($"Item with id {id} not found");
            }
            Material newItem = _mapper.Map<Material>(materialView);
            newItem.MaterialId = id;
            return await _repos.UpdateItemByValue(id, newItem);
        }
    }
}
