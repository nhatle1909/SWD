using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace SWD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OTPController : ControllerBase
    {
        private readonly IOTPService _OTPService;
        public OTPController(IOTPService OTPService, IConfiguration configuration)
        {
            _OTPService = OTPService;
        }
        [HttpPost("Send-SMS")]
        public async Task<IActionResult> SendSMS(string phoneNumber, [FromBody] string message)
        {

            var verification = await _OTPService.SendSMS(phoneNumber, message);
            return Ok(verification);
        }
        //[HttpPost("Send-Verify-OTP")]
        //public async Task<IActionResult> SendOTP(string phoneNumber, string otp)
        //{
        //}

    }
}
