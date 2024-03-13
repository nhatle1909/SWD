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
            if (status.Item1)
                return Ok(status.Item2);
            else return BadRequest(status.Item2);
        }

        [Authorize(Roles = "Staff")]
        [HttpPut("Staff/Update-Interior")]
        public async Task<IActionResult> UpdateInterior(UpdateInteriorView update)
        {
            var status = await _interiorService.UpdateInterior(update);
            if (status.Item1)
                return Ok(status.Item2);
            else return BadRequest(status.Item2);
        }

        [Authorize(Roles = "Staff")]
        [HttpDelete("Staff/Delete-Interior")]
        public async Task<IActionResult> DeleteInterior([FromBody] DeleteInteriorView delete)
        {
            var status = await _interiorService.DeleteInterior(delete);
            if (status.Item1)
                return Ok(status.Item2);
            else return BadRequest(status.Item2);
        }

        [AllowAnonymous]
        [HttpGet("Get-Paging-Interior-List")]
        public async Task<IActionResult> GetPagingInteriorList(int pageIndex, bool isAsc, string? searchValue)
        {
            var status = await _interiorService.GetPagingInterior(pageIndex, isAsc, searchValue);
            return Ok(status);
        }

        [AllowAnonymous]
        [HttpPost("Get-Paging-Interior")]
        public async Task<IActionResult> GetPagingInterior(PagingView request)
        {
            var status = await _interiorService.GetPagingInterior(request.PageIndex, request.IsAsc, request.SearchValue);
            return Ok(status);
        }

        [AllowAnonymous]
        [HttpGet("View-Detail-Interior-From-Paging")]
        public async Task<IActionResult> GetDetailInterior(string interiorId)
        {
            var status = await _interiorService.GetInteriorDetail(interiorId);
            if (status.Item1)
                return Ok(status.Item2);
            else return BadRequest(status.Item2);
        }
    }
}