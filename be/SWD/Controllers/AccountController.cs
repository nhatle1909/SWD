﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Repositories.ModelView;
using Services.Interface;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Security.Claims;
using Twilio.TwiML.Messaging;
using static Repositories.ModelView.AccountView;

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

        [HttpPost("Create-Customer-Account")]
        //[FromBody] method lấy data từ body request, nếu ko sài thì method sẽ lấy data từ url
        public async Task<IActionResult> RegisterAnAccountForCustomer([FromBody] RegisterAccountView register)
        {
            try
            {
                var status = await _ser.AddAnAccountForCustomer(register);
                return Ok(new
                {
                    Message = status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Admin/Create-Staff-Account")]
        public async Task<IActionResult> RegisterAnAccountForStaff([FromBody] RegisterForStaffAccountView registerForStaff)
        {
            try
            {
                var status = await _ser.AddAnAccountForStaff(registerForStaff);
                return Ok(new
                {
                    Message = status
                });
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
                var result = await _ser.LoginByEmailAndPassword(login);
                return Ok(new
                {
                    Message = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Login-Google")]
        public async Task<IActionResult> GoogleLogin(string id_token)
        {
            var status = await _ser.GoogleAuthorizeUser(id_token);
            return Ok(new
            {
                Message = status
            });
        }

        //[HttpPost("Renew-Token")]
        //public async Task<IActionResult> RenewToken(string refreshToken, string accessToken)
        //{
        //    try
        //    {
        //        var status = await _ser.RenewToken(refreshToken, accessToken);
        //        return Ok(status);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [Authorize]
        [HttpPatch("Authorize/Update-An-Account")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateAccountView update)
        {
            try
            {
                // Lấy ID từ JWT
                var id = (HttpContext.User.FindFirst("id")?.Value) ?? "";
                var status = await _ser.UpdateAnAccount(id, update);
                return Ok(new
                {
                    Message = status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPatch("Authorize/Update-Picture-Account")]
        public async Task<IActionResult> UpdatePictureProfile(UpdatePictureAccountView updatePicture)
        {
            try
            {
                var id = (HttpContext.User.FindFirst("id")?.Value) ?? "";
                await _ser.UpdatePictureAccount(id, updatePicture);
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
                var status = await _ser.SendEmailToResetPassword(email);
                return Ok(new
                {
                    Message = status
                });
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
                var status = await _ser.ResetPassword(resetPassword);
                return Ok(new
                {
                    Message = status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("View-Public-Profile")]
        public async Task<IActionResult> ViewProfileAccount([EmailAddress] string email)
        {
            try
            {
                var profile = await _ser.ViewProfile(email);
                return Ok(new
                {
                    Message = profile
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPatch("Authorize/Change-Password")]
        public async Task<IActionResult> ChangePasswordAccount([FromBody] ChangePasswordAccountView changePassword)
        {
            try
            {
                var id = (HttpContext.User.FindFirst("id")?.Value) ?? "";
                var status = await _ser.ChangePassword(id, changePassword);
                return Ok(new
                {
                    Message = status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("Admin/Banned-Account")]
        public async Task<IActionResult> BanAnAccount([FromBody] BanAccountView ban)
        {
            try
            {
                var status = await _ser.BanAnAccount(ban);
                return Ok(new
                {
                    Message = status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Admin/Remove-Account")]
        public async Task<IActionResult> RemoveAnAccount([FromBody] DeleteAccountView delete)
        {
            try
            {
                var status = await _ser.DeleteAnAccount(delete);
                return Ok(new
                {
                    Message = status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("Authorize/Sign-Out")]
        public async Task<IActionResult> SignOutAccount()
        {
            try
            {
                await _ser.SignOutAsync();
                return Ok(new
                {
                    Message = "Sign out successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Admin/Get-Paging-Account-List")]
        public async Task<IActionResult> GetPagingAccountList([FromBody] PagingAccountView paging)
        {
            try
            {
                var status = await _ser.GetPagingAccount(paging);
                return Ok(new
                {
                    Message = status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Admin/View-Private-Detail-Account-From-Paging")]
        public async Task<IActionResult> GetAccountDetail([FromBody] DetailAccountView detail)
        {
            try
            {
                var status = await _ser.GetAccountDetail(detail);
                return Ok(new
                {
                    Message = status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
