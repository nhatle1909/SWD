using Microsoft.AspNetCore.Mvc;
using Repository.Model;
using Repository.ModelView;
using Service.Interface;

namespace SWD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InteriorController : ControllerBase
    {
        private readonly IInteriorService _interiorService;
        public InteriorController(IInteriorService interiorService, IConfiguration configuration)
        {
            _interiorService = interiorService;
        }
        [HttpPost("Add-New-Interior")]
        public async Task<IActionResult> AddOneInterior(InteriorView interiorView)
        {
            Interior item = await _interiorService.AddOneInterior(interiorView);

            return Ok(item);
        }

        [HttpPut("Update-Interior")]
        public async Task<IActionResult> UpdateInterior(string id, InteriorView InteriorView)
        {
            Interior item = await _interiorService.UpdateInterior(id, InteriorView);
            return Ok(item);
        }
        [HttpGet("Get-Paged-Interior-List")]
        public async Task<object> GetPagedInteriorList(int pageIndex, int pageSize, bool isAsc, string sortField, string searchValue, string searchField)
        {
            return await _interiorService.GetPagedInterior(pageIndex, pageSize, isAsc, sortField, searchValue, searchField);
        }
        [HttpGet("Get-All-Interior")]
        public async Task<IActionResult> GetAllInterior()
        {
            IEnumerable<Interior> tempItemList = await _interiorService.GetAllInterior();
            return Ok(tempItemList);
        }
    }
}
