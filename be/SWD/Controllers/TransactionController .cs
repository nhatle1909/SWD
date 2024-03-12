using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.ModelView;
using Services.Interface;
using System.Security.Claims;
using static Repositories.ModelView.CartView;

namespace SWD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _vnpayService;
        public TransactionController(ITransactionService vnpayService)
        {
            _vnpayService = vnpayService;
        }
        [Authorize(Roles = "Staff")]
        [HttpPost("Staff/VNPay-Deposit-Payment")]
        public async Task<IActionResult> VNPayPaymentDeposit(AddCartView[] cartViews)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data Format");
            }
            var id = (HttpContext.User.FindFirst("id")?.Value) ?? "";
            string check = await _vnpayService.AddPendingTransaction(id, cartViews);
            int deposit = await _vnpayService.CalculateDeposit(check);
            if (check != null)
            {
                return Ok(_vnpayService.Payment(check, deposit).Result);
            }
            return BadRequest("Invalid Data Format");
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("Customer/VNPay-Return")]
        public async Task<IActionResult> VNPayReturn(string url)
        {
            var status = await _vnpayService.CheckPayment(url);
            if (status.Item1 == true) return Ok(status.Item2);
            else return BadRequest(status.Item2);
        }
        [HttpGet("Customer/Transaction-List-DO-NOT-USE")]
        public async Task<IActionResult> TransactionList()
        {
            return Ok();
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