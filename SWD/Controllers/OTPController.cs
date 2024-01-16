using EXE.Interface;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.ModelView;

namespace EXE.Controllers
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
        [HttpPost("Add-New-Account")]
        public async Task<IActionResult> SendOTP([FromBody] string phoneNumber) 
        {
            var verification = _OTPService.SendOTP(phoneNumber);
            return Ok(verification);
        }
       
    }
}
