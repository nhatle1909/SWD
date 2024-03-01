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
        [HttpPost("Add-New-Interior")]
        public async Task<IActionResult> AddOneInterior(AddInteriorView add)
        {
            var item = await _interiorService.AddOneInterior(add);

            return Ok(item);
        }

        [HttpPut("Update-Interior")]
        public async Task<IActionResult> UpdateInterior(UpdateInteriorView update)
        {
            var item = await _interiorService.UpdateInterior(update);
            return Ok(item);
        }

        [HttpDelete("Delete-Interior")]
        public async Task<IActionResult> DeleteInterior([FromBody] DeleteInteriorView delete)
        {
            var item = await _interiorService.DeleteInterior(delete);
            return Ok(item);
        }

        [HttpGet("Get-Paging-Interior-List")]
        public async Task<IActionResult> GetPagingInteriorList(int pageIndex, bool isAsc, string? searchValue)
        {
            var result = await _interiorService.GetPagingInterior(pageIndex, isAsc, searchValue);
            return Ok(result);
        }

        [HttpGet("Get-Detail-Interior")]
        public async Task<IActionResult> GetDetailInterior(string interiorId)
        {
            var result = await _interiorService.GetInteriorDetail(interiorId);
            return Ok(result);
        }

    }
}
