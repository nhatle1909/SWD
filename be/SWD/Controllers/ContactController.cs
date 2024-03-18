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

        [HttpPost("Staff/Address-An-Request")]
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

        [HttpPost("Get-Paging-Request-List")]
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
                    return Content("<html><body style='display: flex; justify-content: center; align-items: center; height: 100vh; color: #333; background-color: white;'><div style=\"background-color: #f0f9ff; border: 1px solid #e0e0e0; border-radius: 5px; padding: 20px; text-align: center; margin: 20px auto; width: 50%;\">\r\n  <h1 style=\"font-size: 24px; color: #333; margin-bottom: 10px;\">Thank you for accepting this order, you will receive a contract file and a deposit link of 30% of the order value.!</h1>\r\n  <p style=\"font-size: 16px; color: #666;\">We appreciate your business and hope you were satisfied with your experience.</p>\r\n</div></body></html>", "text/html");
                else if (!status.Item1 && status.Item2.Equals("false"))
                    return Content("<html><body style='display: flex; justify-content: center; align-items: center; height: 100vh; color: #333; background-color: white;'><div style=\"background-color: #f0f9ff; border: 1px solid #e0e0e0; border-radius: 5px; padding: 20px; text-align: center; margin: 20px auto; width: 50%;\">\r\n  <h1 style=\"font-size: 24px; color: #333; margin-bottom: 10px;\">You have rejected this order before, if you want to create another order, go to the Interior quotation system home page to create a new order!</h1>\r\n  <p style=\"font-size: 16px; color: #666;\">We appreciate your business and hope you were satisfied with your experience.</p>\r\n</div></body></html>", "text/html");
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
                    return Content("<html><body style='display: flex; justify-content: center; align-items: center; height: 100vh; color: #333; background-color: white;'><div style=\"background-color: #f0f9ff; border: 1px solid #e0e0e0; border-radius: 5px; padding: 20px; text-align: center; margin: 20px auto; width: 50%;\">\r\n  <h1 style=\"font-size: 24px; color: #333; margin-bottom: 10px;\">We're sorry you refused the order!</h1>\r\n  <p style=\"font-size: 16px; color: #666;\">We appreciate your business and hope you were satisfied with your experience.</p>\r\n</div></body></html>", "text/html");
                else return BadRequest(status.Item2);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
