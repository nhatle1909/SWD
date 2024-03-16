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

        [HttpPost("Add-An-Request-For-Guest")]
        public async Task<IActionResult> AddContactForGuest(AddContactView add)
        {
            try
            {
                var status = await _contactService.AddContactForGuest(add);
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
        [HttpPost("Add-An-Request-For-Customer")]
        public async Task<IActionResult> AddContactForCustomer(AddForCustomerContactView add)
        {
            try
            {
                var id = (HttpContext.User.FindFirst("id")?.Value) ?? "";
                var status = await _contactService.AddContactForCustomer(id, add);
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
        [HttpPut("Staff/Address-An-Request")]
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
        [HttpDelete("Staff/Delete-An-Request")]
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
        [HttpPost("Staff/Get-Paging-Request-List")]
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

        //[Authorize(Roles = "Staff")]
        //[HttpPost("Staff/View-Private-Detail-Request-From-Paging")]
        //public async Task<IActionResult> GetContactDetail(DetailContactView detail)
        //{
        //    try
        //    {
        //        var status = await _contactService.GetContactDetail(detail);
        //        if (status.Item1)
        //            return Ok(status.Item2);
        //        else return BadRequest(status.Item2);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        //[Authorize(Roles = "Staff")]
        //[HttpPost("Staff/Create-Contract-PDF")]
        //public async Task<IActionResult> Test(string contactId, AddCartView[] array)
        //{
        //    var status = await _contactService.GenerateContractPdf(contactId, array);
        //    if (status.Item1 == false) return BadRequest("Error");
        //    else return Ok(status.Item3);
        //}

        [Authorize(Roles = "Customer")]
        [HttpPost("Customer/Get-Customer-Request-List")]
        public async Task<IActionResult> GetAllRequestCustomer()
        {
            var _id = (HttpContext.User.FindFirst("id")?.Value) ?? "";
            var status = await _contactService.GetCustomerContactList(_id);
            if (status.Item1 == false) return BadRequest("Invalid Token || Mail does not exist");
            else return Ok(status.Item2);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("Customer/View-Detail-Customer-Request")]
        public async Task<IActionResult> ViewDetailRequestCustomer(DetailContactView detail)
        {
            try
            {
                var status = await _contactService.GetCustomerContactDetail(detail);
                if (status.Item1)
                    return Ok(status.Item2);
                else return BadRequest(status.Item2);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Accepted")]
        public async Task<IActionResult> AcceptedQuote([FromQuery] string requestId)
        {
            try
            {
                var status = await _contactService.Accepted(requestId);
                if (status.Item1)
                    return Ok();
                else return BadRequest(status.Item2);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Refused")]
        public async Task<IActionResult> RefusedQuote([FromQuery] string requestId)
        {
            try
            {
                var status = await _contactService.Refused(requestId);
                if (status.Item1)
                    return Ok();
                else return BadRequest(status.Item2);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
