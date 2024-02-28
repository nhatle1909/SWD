using Repositories.Model;
using Repositories.ModelView;
using static Repositories.ModelView.InteriorView;

namespace Services.Interface
{
    public interface IInteriorService
    {
        Task<object> GetPagingInterior(int pageIndex, bool isAsc, string? searchValue);
        Task<Interior?> GetInteriorDetail(string interiorId);
        Task<string> AddOneInterior(AddInteriorView add);
        Task<string> UpdateInterior(UpdateInteriorView update);
        Task<string> DeleteInterior(DeleteInteriorView delete);
    }
}
