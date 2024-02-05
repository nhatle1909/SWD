using Microsoft.AspNetCore.Mvc;
using Repository.Model;
using Repository.ModelView;
using Service.Interface;
namespace SWD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {

        private readonly ITemplateService _TempService;
        public TemplateController(ITemplateService TemplateService)
        {
            _TempService = TemplateService;
        }
        [HttpPost("Add-One-Item-Template")]
        public async Task<IActionResult> AddOneTemplateItem(TemplateModelView item)
        {
            TemplateModel tempItem = await _TempService.AddOneTemplateItem(item);
            return Ok(tempItem);
        }
        [HttpGet("Get-All-Item-Template")]
        public async Task<IActionResult> GetAllTemplateItem()
        {
            IEnumerable<TemplateModel> tempItemList = await _TempService.GetAllTemplateItem();
            return Ok(tempItemList);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateTemplate(string id, TemplateModelView TemplateModelView)
        {
            TemplateModel tempItem = await _TempService.UpdateTemplateItem(id, TemplateModelView);
            return Ok(tempItem);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteTemplate(string id)
        {
            bool isDeleted = await _TempService.DeleteTemplateItem(id);
            if (isDeleted)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
