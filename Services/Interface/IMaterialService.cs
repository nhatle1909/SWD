using Repositories.Model;
using Repositories.ModelView;
using static Repositories.ModelView.MaterialView;

namespace Services.Interface
{
    public interface IMaterialService
    {
        Task<object> GetPagingMaterial(PagingMaterialView paging);
        Task<string> AddOneMaterial(AddMaterialView add);
        Task<string> UpdateMaterial(UpdateMaterialView update);
        Task<string> DeleteMaterial(DeleteMaterialView delete);
    }
}
