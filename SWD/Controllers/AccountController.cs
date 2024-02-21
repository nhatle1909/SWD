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

        [HttpPost("Login-By-Email-Password")]
        public async Task<IActionResult> LoginAccountByEmailAndPassword(LoginAccountView login)
        {
            try
            {
                string jwt = await _ser.LoginByEmailAndPassword(login);
                return Ok(jwt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("Update-An-Account")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateAccountView update)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string status = await _ser.UpdateAnAccount(update);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("Update-Picture-Account")]
        public async Task<IActionResult> UpdatePictureProfile([FromBody] UpdatePictureAccountView updatePicture)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _ser.UpdatePictureAccount(updatePicture);
                return Ok();
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
        public async Task<IActionResult> ResetPasswordAccount([FromBody] ResetPasswordAccountView resetPassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string notice = await _ser.ResetPassword(resetPassword);
                return Ok(notice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("View-Profile")]
        public async Task<IActionResult> ViewProfileAccount([FromBody] ViewProfileAccountView viewProfile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var profile = await _ser.ViewProfile(viewProfile);
                if (profile.Item1 is null && profile.Item2 is null) return Ok("null");
                var tuple = new
                {
                    Email = profile.Item1!.Email,
                    PhoneNumber = profile.Item1.PhoneNumber,
                    Address = profile.Item1.Address,
                    Picture = profile.Item1.Picture,
                    CreatedAt = profile.Item2!.CreatedAt,
                    UpdatedAt = profile.Item2.UpdatedAt
                };
                return Ok(tuple);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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

        [HttpPost("Sign-Out")]
        public async Task<IActionResult> SignOutAccount()
        {
            try
            {
                await _ser.SignOutAsync();
                return Ok("Sign out successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
