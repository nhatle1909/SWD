using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using Services.Service;
using static Repositories.ModelView.ContactView;

namespace SWD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : Controller
    {
        private IContactService _contactService;
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpPost("Add-An-Contact")]
        public async Task<IActionResult> AddAnContact(AddContactView add)
        {
            try
            {
                var status = await _contactService.AddContact(add);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Address-An-Contact")]
        public async Task<IActionResult> AddressAnContact(AddressContactView address)
        {
            try
            {
                var status = await _contactService.AddressTheContact(address);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete-An-Contact")]
        public async Task<IActionResult> DeleteAnContact(DeleteContactView delete)
        {
            try
            {
                var status = await _contactService.DeleteContact(delete);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Get-Paging-Contact-List")]
        public async Task<IActionResult> GetPagingContactlList(PagingContactView paging)
        {
            try
            {
                var status = await _contactService.GetPagingContact(paging);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Get-Contact-Detail")]
        public async Task<IActionResult> GetContactDetail(DetailContactView detail)
        {
            try
            {
                var status = await _contactService.GetContactDetail(detail);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
