using Microsoft.AspNetCore.Mvc;
using Repositories.ModelView;
using Services.Interface;

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

        [HttpPost("VNPay-Payment")]
        public async Task<IActionResult> VNPayPayment(PaymentView paymentView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data Format");
            }
            string check = await _vnpayService.AddPendingRequest(paymentView);
            if (check != null)
            {
                return Ok(_vnpayService.Payment(check, paymentView).Result);
            }
            return BadRequest("Email has not authenticated");
        }
        [HttpGet("VNPay-Return")]
        public async Task<IActionResult> VNPayReturn(string url)
        {
            return Ok(await _vnpayService.CheckPayment(url));
        }



    }
}
