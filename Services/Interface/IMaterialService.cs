using Repository.Model;
using Repository.ModelView;
using static Repository.ModelView.MaterialView;

namespace Service.Interface
{
    public interface IMaterialService
    {
        public Task<IEnumerable<Material>> GetAllMaterial();
        public Task<object> GetPagedMaterial(int pageIndex, int pageSize, bool isAsc, string sortField, string searchValue, string searchField);
        public Task<string> AddOneMaterial(AddMaterialView add);
        public Task<string> UpdateMaterial(UpdateMaterialView update);
        public Task<string> DeleteMaterial(DeleteMaterialView delete);
    }
}
