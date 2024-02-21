using Microsoft.AspNetCore.Mvc;
using Repository.Model;
using Repository.ModelView;
using Service.Interface;
using static Repository.ModelView.MaterialView;
namespace SWD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {

        private readonly IMaterialService _Service;
        public MaterialController(IMaterialService Service)
        {
            _Service = Service;
        }

        [HttpGet("Get-Paged-Material-List")]
        public async Task<object> GetPagedMaterialList(int pageIndex, int pageSize, bool isAsc, string searchValue, string searchField)
        {
            return await _Service.GetPagedMaterial(pageIndex, pageSize, isAsc, "MaterialName", searchValue, searchField);
        }

        [HttpGet("Get-All-Material")]
        public async Task<IActionResult> GetAllMaterial()
        {
            IEnumerable<Material> tempItemList = await _Service.GetAllMaterial();
            return Ok(tempItemList);
        }

        [HttpPost("Add-One-Material")]
        public async Task<IActionResult> AddOneMaterial(AddMaterialView add)
        {
            string status = await _Service.AddOneMaterial(add);
            return Ok(status);
        }

        [HttpPut("Update-Material")]
        public async Task<IActionResult> UpdateMaterial(UpdateMaterialView update)
        {
            string status = await _Service.UpdateMaterial(update);
            return Ok(status);
        }

        [HttpDelete("Delete-Material")]
        public async Task<IActionResult> DeleteMaterial(DeleteMaterialView delete)
        {
            string status = await _Service.DeleteMaterial(delete);
            return Ok(status);
        }
    }
}
