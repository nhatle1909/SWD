using AutoMapper;
using MongoDB.Bson;
using Repository.Model;
using Repository.ModelView;
using Repository.Repository;
using Service.Interface;

namespace Service.Service
{

    public class TemplateService : ITemplateService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<TemplateModel> _repos;
        public TemplateService(IRepository<TemplateModel> TemplateRepo, IMapper mapper)
        {

            _repos = TemplateRepo;
            _mapper = mapper;
        }
        public async Task<TemplateModel> AddOneTemplateItem(TemplateModelView TemplateModelView)
        {
            TemplateModel TemplateItem = _mapper.Map<TemplateModel>(TemplateModelView);
            TemplateItem.id = ObjectId.GenerateNewId().ToString();
            return await _repos.AddOneItem(TemplateItem);
        }

        public async Task<bool> DeleteTemplateItem(string id)
        {
            IEnumerable<TemplateModel> selectedItem = await _repos.GetByFilterAsync(a => a.id.Equals(id));
            if (!selectedItem.Any())
            {
                throw new Exception($"Item with id {id} not found");
            }

            return await _repos.RemoveItemByValue(id);
        }

        public async Task<IEnumerable<TemplateModel>> GetAllTemplateItem()
        {
            return await _repos.GetAllAsync();
        }

        public async Task<object> GetPagedTemplateItem(int pageIndex, int pageSize, bool isAsc, string sortField, string searchValue, string searchField)
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
                Template = item.ToList()
            };
            return response;
        }

        public async Task<TemplateModel> UpdateTemplateItem(string id, TemplateModelView TemplateModelView)
        {
            IEnumerable<TemplateModel> selectedItem = await _repos.GetByFilterAsync(a => a.id.Equals(id));
            if (!selectedItem.Any())
            {
                throw new Exception($"Item with id {id} not found");
            }
            TemplateModel TemplateItem = _mapper.Map<TemplateModel>(TemplateModelView);
            TemplateItem.id = id;
            return await _repos.UpdateItemByValue(id, TemplateItem);
        }
    }

}
