using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Model;
using Repositories.ModelView;
using Services.Interface;
using static Repositories.ModelView.InteriorView;

namespace SWD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InteriorController : ControllerBase
    {
        private readonly IInteriorService _interiorService;
        public InteriorController(IInteriorService interiorService)
        {
            _interiorService = interiorService;
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("Staff/Add-New-Interior")]
        public async Task<IActionResult> AddOneInterior(AddInteriorView add)
        {
            var status = await _interiorService.AddOneInterior(add);
            return Ok(new
            {
                Message = status
            });
        }

        [Authorize(Roles = "Staff")]
        [HttpPut("Staff/Update-Interior")]
        public async Task<IActionResult> UpdateInterior(UpdateInteriorView update)
        {
            var status = await _interiorService.UpdateInterior(update);
            return Ok(new
            {
                Message = status
            });
        }

        [Authorize(Roles = "Staff")]
        [HttpDelete("Staff/Delete-Interior")]
        public async Task<IActionResult> DeleteInterior([FromBody] DeleteInteriorView delete)
        {
            var status = await _interiorService.DeleteInterior(delete);
            return Ok(new
            {
                Message = status
            });
        }

        [AllowAnonymous]
        [HttpGet("Get-Paging-Interior-List")]
        public async Task<IActionResult> GetPagingInteriorList(int pageIndex, bool isAsc, string? searchValue)
        {
            var status = await _interiorService.GetPagingInterior(pageIndex, isAsc, searchValue);
            return Ok(new
            {
                Message = status
            });
        }

        [AllowAnonymous]
        [HttpGet("View-Detail-Interior-From-Paging")]
        public async Task<IActionResult> GetDetailInterior(string interiorId)
        {
            var status = await _interiorService.GetInteriorDetail(interiorId);
            return Ok(new
            {
                Message = status
            });
        }

    }
}
