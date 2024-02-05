using Microsoft.AspNetCore.Mvc;
using Repository.Model;
using Repository.ModelView;
using Service.Interface;

namespace SWD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
        }
        [HttpPost("Add-New-Account")]
        public async Task<IActionResult> AddOneAccount(AccountView accountView)
        {
            Account item = await _accountService.AddOneAccount(accountView);

            return Ok(item);
        }
        [HttpPut("Ban-An-Account")]
        public async Task<IActionResult> BanAccount(string Id, AccountView account)
        {
            Account item = await _accountService.BannedOneAccount(Id, account);
            return Ok(item);
        }
        [HttpPut("Update-Account")]
        public async Task<IActionResult> UpdateAccount(string id, AccountView accountView)
        {
            Account item = await _accountService.UpdateAccount(id, accountView);
            return Ok(item);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser(string Username, string Password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            (string, AccountView) tuple = await _accountService.Login(Username, Password);
            if (tuple.Item1 == null)
            {
                return Unauthorized();
            }

            Dictionary<string, object> result = new()
            {
                { "token", tuple.Item1 },
                { "user", tuple.Item2 }
            };
            return Ok(result);
        }
    }
}
