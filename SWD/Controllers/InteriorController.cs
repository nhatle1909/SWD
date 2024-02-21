using Microsoft.AspNetCore.Mvc;
using Repository.Model;
using Repository.ModelView;
using Service.Interface;
using static Repository.ModelView.InteriorView;

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
        public async Task<IActionResult> DeleteInterior(DeleteInteriorView delete)
        {
            var item = await _interiorService.DeleteInterior(delete);
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

        //[HttpPost("Optional-Interior-Quote")]
        //public async Task<IActionResult> OptionalInteriorQuote(string[] arrMaterialId)
        //{
        //    double totalPrice = await _interiorService.OptionalInteriorQuote(arrMaterialId);

        //    return Ok(totalPrice);
        //}
    }
}
