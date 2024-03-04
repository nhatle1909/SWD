using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Staff")]
        [HttpPost("Staff/Get-Paging-Material-List")]
        public async Task<object> GetPagingMaterialList([FromBody] PagingMaterialView paging)
        {
            var status = await _Service.GetPagingMaterial(paging);
            return Ok(new
            {
                Message = status
            });
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("Staff/Add-One-Material")]
        public async Task<IActionResult> AddOneMaterial([FromBody] AddMaterialView add)
        {
            string status = await _Service.AddOneMaterial(add);
            return Ok(new
            {
                Message = status
            });
        }

        [Authorize(Roles = "Staff")]
        [HttpPut("Staff/Update-Material")]
        public async Task<IActionResult> UpdateMaterial([FromBody] UpdateMaterialView update)
        {
            string status = await _Service.UpdateMaterial(update);
            return Ok(new
            {
                Message = status
            });
        }

        [Authorize(Roles = "Staff")]
        [HttpDelete("Staff/Delete-Material")]
        public async Task<IActionResult> DeleteMaterial([FromBody] DeleteMaterialView delete)
        {
            string status = await _Service.DeleteMaterial(delete);
            return Ok(new
            {
                Message = status
            });
        }

        //httpget chỉ hỗ trợ những kiểu có thể parse sang string
        [Authorize]
        [HttpPost("Authorize/Optional-Product-Quote")]
        public async Task<IActionResult> OptionalInteriorQuote(string[] arrMaterialId)
        {
            double totalPrice = await _Service.OptionalProductQuote(arrMaterialId);

            return Ok(totalPrice);
        }

    }
}
