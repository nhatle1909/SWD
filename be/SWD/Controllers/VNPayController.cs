using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.ModelView;
using Services.Interface;
using System.Security.Claims;

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
        [HttpPost("Customer/VNPay-Payment")]
        public async Task<IActionResult> VNPayPayment(int totalPrice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data Format");
            }
            var id = (HttpContext.User.FindFirst("id")?.Value) ?? "";
            string check = await _vnpayService.AddPendingRequest(id, totalPrice);
            if (check != null)
            {
                return Ok(_vnpayService.Payment(check, totalPrice).Result);
            }
            return BadRequest("Email has not authenticated");
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("Customer/VNPay-Return")]
        public async Task<IActionResult> VNPayReturn(string url)
        {
            return Ok(await _vnpayService.CheckPayment(url));
        }



    }
}