using Repositories.Model;
using Repositories.ModelView;
using static Repositories.ModelView.InteriorView;

namespace Services.Interface
{
    public interface IInteriorService
    {
        Task<object> GetPagingInterior(int pageIndex, bool isAsc, string? searchValue);
        Task<(bool, object)> GetInteriorDetail(string interiorId);
        Task<(bool, string)> AddOneInterior(AddInteriorView add);
        Task<(bool, string)> UpdateInterior(UpdateInteriorView update);
        Task<(bool, string)> DeleteInterior(DeleteInteriorView delete);
    }
}
