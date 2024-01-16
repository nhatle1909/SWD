using Models.Model;
using Models.ModelView;

namespace EXE.Interface
{
    public interface ITemplateService
    {
        public Task<IEnumerable<TemplateModel>> GetAllTemplateItem();
        public Task<TemplateModel> AddOneTemplateItem(TemplateModelView TemplateModelView);
        public Task<TemplateModel> UpdateTemplateItem(string id, TemplateModelView TemplateModelView);
        public Task<bool> DeleteTemplateItem(string id);
    }
}
