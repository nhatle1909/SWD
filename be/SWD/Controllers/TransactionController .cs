using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Model;
using Repositories.ModelView;
using Services.Interface;
using System.Net.NetworkInformation;
using System.Security.Claims;
using static Repositories.ModelView.CartView;

namespace SWD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _vnpayService;
        private readonly IContactService _contactService;
        public TransactionController(ITransactionService vnpayService, IContactService contactService)
        {
            _vnpayService = vnpayService;
            _contactService = contactService;
        }
        [Authorize(Roles = "Staff")]
        [HttpPost("Staff/VNPay-Deposit-Payment")]
        public async Task<IActionResult> VNPayPaymentDeposit(string contactId, AddCartView[] cartViews)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data Format");
            }
            var id = (HttpContext.User.FindFirst("id")?.Value) ?? "";
            string check = await _vnpayService.AddPendingTransaction(contactId, cartViews);
            int deposit = await _vnpayService.CalculateDeposit(check);
        
            var status2 = await _contactService.UpdateContact(contactId, cartViews);
            if (check != null)
            {
                 if (status2.Item1)
                    {
                        return Ok(_vnpayService.Payment(check, deposit).Result);
                    }
                    return BadRequest(status2.Item2);
               
               
            }
            return BadRequest("Invalid Data Format");
        }

        [HttpGet("VNPay-Return")]
        public async Task<IActionResult> VNPayReturn(string url)
        {
            var status = await _vnpayService.CheckPayment(url);
            if (status.Item1 == true) return Ok(status.Item2);
            else return BadRequest(status.Item2);
        }
        [HttpGet("Customer/Get-Transaction-List")]
        public async Task<IActionResult> TransactionList()
        {

            var id = (HttpContext.User.FindFirst("id")?.Value) ?? "";
         
                var status = await _vnpayService.GetAllTransaction(id);
            if (status.Item1 == false) return BadRequest("Account does not exist | Invalid Token");
            else return Ok(status.Item2);
        }
        //[Authorize(Roles = "Staff")]
        //[HttpPost("Staff/Update-Request-Item")]
        //public async Task<IActionResult> UpdateRequestDetail(string _id, AddCartView[] cartviews)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("Invalid Data");
        //    }
        //    return Ok(await _vnpayService.UpdateTransactionDetail(_id, cartviews));
        //}
        [Authorize(Roles = "Staff")]
        [HttpPost("Staff/Delete-Expired-Request")]
        public async Task<IActionResult> DeleteExpiredTransaction(string[] _id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }
            return Ok(await _vnpayService.DeleteExpiredTransaction(_id));
        }



    }
}