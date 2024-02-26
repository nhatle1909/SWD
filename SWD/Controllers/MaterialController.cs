using Microsoft.AspNetCore.Mvc;
using Repositories.Model;
using Repositories.ModelView;
using Services.Interface;
using static Repositories.ModelView.MaterialView;
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

        [HttpPost("Get-Paging-Material-List")]
        public async Task<object> GetPagingMaterialList([FromBody] PagingMaterialView paging)
        {
            return await _Service.GetPagingMaterial(paging);
        }

        [HttpPost("Add-One-Material")]
        public async Task<IActionResult> AddOneMaterial([FromBody] AddMaterialView add)
        {
            string status = await _Service.AddOneMaterial(add);
            return Ok(status);
        }

        [HttpPut("Update-Material")]
        public async Task<IActionResult> UpdateMaterial([FromBody] UpdateMaterialView update)
        {
            string status = await _Service.UpdateMaterial(update);
            return Ok(status);
        }

        [HttpDelete("Delete-Material")]
        public async Task<IActionResult> DeleteMaterial([FromBody] DeleteMaterialView delete)
        {
            string status = await _Service.DeleteMaterial(delete);
            return Ok(status);
        }
    }
}
