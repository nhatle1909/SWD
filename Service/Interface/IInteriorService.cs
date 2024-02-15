using Repository.Model;
using Repository.ModelView;

namespace Service.Interface
{
    public interface IInteriorService
    {
        public Task<IEnumerable<Interior>> GetAllInterior();
        public Task<object> GetPagedInterior(int pageIndex, int pageSize, bool isAsc, string sortField, string searchValue, string searchField);
        public Task<Interior> AddOneInterior(InteriorView interiorView);
        public Task<Interior> UpdateInterior(string id, InteriorView interiorView);
        public Task<bool> DeleteInterior(string id);
    }
}
