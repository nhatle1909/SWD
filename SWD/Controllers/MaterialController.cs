using Microsoft.AspNetCore.Mvc;
using Repository.Model;
using Repository.ModelView;
using Service.Interface;
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
        [HttpPost("Add-One-Material")]
        public async Task<IActionResult> AddOneMaterial(MaterialView item)
        {
            Material Item = await _Service.AddOneMaterial(item);
            return Ok(Item);
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
        [HttpPut("Update-Material")]
        public async Task<IActionResult> UpdateMaterial(string id, MaterialView materialView)
        {
            Material tempItem = await _Service.UpdateMaterial(id, materialView);
            return Ok(tempItem);
        }
        [HttpDelete("Delete-Material")]
        public async Task<IActionResult> DeleteMaterial(string id)
        {
            bool isDeleted = await _Service.DeleteMaterial(id);
            if (isDeleted)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
