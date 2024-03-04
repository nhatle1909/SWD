using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
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

        [AllowAnonymous] 
        [HttpPost("Add-An-Contact")]
        public async Task<IActionResult> AddAnContact(AddContactView add)
        {
            try
            {
                var status = await _contactService.AddContact(add);
                return Ok(new
                {
                    Message = status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpPut("Staff/Address-An-Contact")]
        public async Task<IActionResult> AddressAnContact(AddressContactView address)
        {
            try
            {
                var status = await _contactService.AddressTheContact(address);
                return Ok(new
                {
                    Message = status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpDelete("Staff/Delete-An-Contact")]
        public async Task<IActionResult> DeleteAnContact(DeleteContactView delete)
        {
            try
            {
                var status = await _contactService.DeleteContact(delete);
                return Ok(new
                {
                    Message = status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("Staff/Get-Paging-Contact-List")]
        public async Task<IActionResult> GetPagingContactlList(PagingContactView paging)
        {
            try
            {
                var status = await _contactService.GetPagingContact(paging);
                return Ok(new
                {
                    Message = status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("Staff/View-Private-Detail-Contact-From-Paging")]
        public async Task<IActionResult> GetContactDetail(DetailContactView detail)
        {
            try
            {
                var status = await _contactService.GetContactDetail(detail);
                return Ok(new
                {
                    Message = status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
