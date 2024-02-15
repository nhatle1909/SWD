using Repository.Model;
using Repository.ModelView;

namespace Service.Interface
{
    public interface ITemplateService
    {
        public Task<IEnumerable<TemplateModel>> GetAllTemplateItem();
        public Task<object> GetPagedTemplateItem(int pageIndex, int pageSize, bool isAsc, string sortField, string searchValue, string searchField);
        public Task<TemplateModel> AddOneTemplateItem(TemplateModelView TemplateModelView);
        public Task<TemplateModel> UpdateTemplateItem(string id, TemplateModelView TemplateModelView);
        public Task<bool> DeleteTemplateItem(string id);
    }
}
