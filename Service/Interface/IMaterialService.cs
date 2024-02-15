using Repository.Model;
using Repository.ModelView;

namespace Service.Interface
{
    public interface IMaterialService
    {
        public Task<IEnumerable<Material>> GetAllMaterial();
        public Task<object> GetPagedMaterial(int pageIndex, int pageSize, bool isAsc, string sortField, string searchValue, string searchField);
        public Task<Material> AddOneMaterial(MaterialView materialView);
        public Task<Material> UpdateMaterial(string id, MaterialView materialView);
        public Task<bool> DeleteMaterial(string id);
    }
}
