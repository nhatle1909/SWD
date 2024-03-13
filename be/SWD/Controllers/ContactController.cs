using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Repositories.Model;
using Services.Interface;
using Twilio.Rest.Video.V1.Room.Participant;
using static Repositories.ModelView.CartView;
using static Repositories.ModelView.ContactView;

namespace SWD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpPost("Add-An-Contact-For-Guest")]
        public async Task<IActionResult> AddContactForGuest(string interiorId, AddContactView add)
        {
            try
            {
                var status = await _contactService.AddContactForGuest( interiorId, add);
                if (status.Item1)
                    return Ok(status.Item2);
                else return BadRequest(status.Item2);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("Add-An-Contact-For-Customer")]
        public async Task<IActionResult> AddContactForCustomer(string interiorId, AddForCustomerContactView add)
        {
            try
            {
                var id = (HttpContext.User.FindFirst("id")?.Value) ?? "";
                var status = await _contactService.AddContactForCustomer(id, interiorId, add);
                if (status.Item1)
                    return Ok(status.Item2);
                else return BadRequest(status.Item2);
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
                if (status.Item1)
                    return Ok(status.Item2);
                else return BadRequest(status.Item2);
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
                if (status.Item1)
                    return Ok(status.Item2);
                else return BadRequest(status.Item2);
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
                return Ok(status);
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
                if (status.Item1)
                    return Ok(status.Item2);
                else return BadRequest(status.Item2);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles ="Staff")]
        [HttpPost("Staff/Create-Contract-PDF")]
        public async Task<IActionResult> Test(string staffId, string contactId, AddCartView[] array) 
        {
            var status = await _contactService.GenerateContractPdf(staffId, contactId, array);
            if (status.Item1 == false) return BadRequest("Error");
            else return Ok(status.Item3);
        }
        [Authorize(Roles = "Customer")]
        [HttpGet("Customer/Get-Customer-Request-List")]
        public async Task<IActionResult> GetAllRequestCustomer() 
        {
            var _id = (HttpContext.User.FindFirst("id")?.Value) ?? "";
            var status = await _contactService.GetCustomerContactList(_id);
            if (status.Item1 == false) return BadRequest("Invalid Token || Mail does not exist");
            else return Ok(status.Item2);
        }


    }
}
