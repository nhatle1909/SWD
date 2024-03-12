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
    public class VNPayController : ControllerBase
    {
        private readonly IRequestService _vnpayService;
        public VNPayController(IRequestService vnpayService)
        {
            _vnpayService = vnpayService;
        }
        [Authorize(Roles = "Customer")]
        [HttpPost("Customer/VNPay-Deposit-Payment")]
        public async Task<IActionResult> VNPayPaymentDeposit(AddCartView[] cartViews)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data Format");
            }
            var id = (HttpContext.User.FindFirst("id")?.Value) ?? "";
            string check = await _vnpayService.AddPendingRequest(id, cartViews);
            int deposit = await _vnpayService.CalculateDeposit(check);
            if (check != null)
            {
                return Ok(_vnpayService.Payment(check,deposit).Result);
            }
            return BadRequest("Email has not authenticated");
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("Customer/VNPay-Return")]
        public async Task<IActionResult> VNPayReturn(string url)
        {
            return Ok(await _vnpayService.CheckPayment(url));
        }
        [Authorize(Roles ="Customer")]
        [HttpPost("Customer/VNPay-RemainPrice-Payment")]
        public async Task<IActionResult> VNPayPaymentRemainPrice(string requestId) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest("Invalid Data");
            }
            int remainPrice = await _vnpayService.GetRemainPrice(requestId);

            return Ok(_vnpayService.Payment(requestId,remainPrice).Result);
        }
        [Authorize(Roles = "Customer")]
        [HttpPost("Customer/Update-Request-Item")]
        public async Task<IActionResult> UpdateRequestDetail(string _id, AddCartView[] cartviews)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }
            return Ok(await _vnpayService.UpdateRequestDetail(_id,cartviews));
        }
        [Authorize(Roles = "Customer")]
        [HttpPost("Customer/Delete-Expired-Request")]
        public async Task<IActionResult> DeleteExpiredRequest(string[] _id) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest("Invalid Data");
            }
            return Ok(await _vnpayService.DeleteExpiredRequest(_id));
        }



    }
}