using Repository.Model;
using Repository.ModelView;
using static Repository.ModelView.InteriorView;

namespace Service.Interface
{
    public interface IInteriorService
    {
        public Task<IEnumerable<Interior>> GetAllInterior();
        public Task<object> GetPagedInterior(int pageIndex, int pageSize, bool isAsc, string sortField, string searchValue, string searchField);
        public Task<string> AddOneInterior(AddInteriorView add);
        public Task<string> UpdateInterior(UpdateInteriorView update);
        public Task<string> DeleteInterior(DeleteInteriorView delete);
        //public Task<double> OptionalInteriorQuote(string[] arrMaterialId);
    }
}
