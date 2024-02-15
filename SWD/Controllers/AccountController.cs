using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System.ComponentModel.DataAnnotations;
using static Repository.ModelView.AccountView;

namespace SWD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {

        private readonly IAccountService _ser;
        public AccountController(IAccountService ser)
        {
            _ser = ser;
        }

        [HttpPost("Register-Customer-Account")]
        //[FromBody] method lấy data từ body request, nếu ko sài thì method sẽ lấy data từ url
        public async Task<IActionResult> RegisterAnAccountForCustomer([FromBody] RegisterAccountView register)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var status = await _ser.AddAnAccountForCustomer(register);
                return Ok(new { message = $"{status}" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize]

        [HttpPost("Register-Staff-Account")]
        public async Task<IActionResult> RegisterAnAccountForStaff([FromBody] RegisterForStaffAccountView registerForStaff)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var status = await _ser.AddAnAccountForStaff(registerForStaff);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAccountByUsernameAndPassword(LoginAccountView login)
        {
            try
            {
                string jwt = await _ser.LoginByUsernameAndPassword(login);
                return Ok(jwt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("Update-Account")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateAccountView update)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string jwt = await _ser.UpdateAnAccount(update);
                return Ok(jwt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Send-Mail-To-Reset-Password")]
        public async Task<IActionResult> SendEmailToResetPasswordAccount([EmailAddress] string email)
        {
            try
            {
                string notice = await _ser.SendEmailToResetPassword(email);
                return Ok(notice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPasswordAccount([FromBody] ResetPasswordAccountView resetPasswordAccountView)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string notice = await _ser.ResetPassword(resetPasswordAccountView);
                return Ok(notice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize]
        [HttpPatch("Change-Password")]
        public async Task<IActionResult> ChangePasswordAccount([FromBody] ChangePasswordAccountView changePassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string notice = await _ser.ChangePassword(changePassword);
                return Ok(notice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("Banned-Account")]
        public async Task<IActionResult> BanAnAccount([FromBody] BanAccountView ban)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string notice = await _ser.BanAnAccount(ban);
                return Ok(notice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Remove-Account")]
        public async Task<IActionResult> RemoveAnAccount([FromBody] DeleteAccountView delete)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string notice = await _ser.DeleteAnAccount(delete);
                return Ok(notice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
