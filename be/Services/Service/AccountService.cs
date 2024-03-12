using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Repositories.Model;
using Repositories.Models;
using Repositories.Repository;
using Services.Tool;
using Services.Interface;
using System.IO;
using System.Net.NetworkInformation;
using System.Web;
using static Repositories.ModelView.AccountView;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Repositories.ModelView;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Google.Apis.Auth;

namespace Services.Service
{

    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountService> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountService(IUnitOfWork unit, IConfiguration configuration, IMapper mapper, ILogger<AccountService> logger, IEmailSender emailSender, IHttpContextAccessor httpContextAccessor)
        {
            _unit = unit;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(bool, string)> AddAnAccountForCustomer(RegisterAccountView register)
        {
            IEnumerable<Account> check = await _unit.AccountRepo.GetFieldsByFilterAsync(["Email"],
                            c => c.Email.Equals(register.Email.Trim()));
            if (!check.Any())
            {
                //Encode picture
                byte[] fileBytes;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AccountPictures", "user_default.png");
                using (var ms = new MemoryStream())
                {
                    using (var defaultUserImg = new FileStream(path, FileMode.Open))
                    {
                        await defaultUserImg.CopyToAsync(ms);
                        fileBytes = ms.ToArray();
                    }
                }
                string id = ObjectId.GenerateNewId().ToString();
                Account account = _mapper.Map<Account>(register);
                account.AccountId = id;
                account.Email = register.Email.Trim();
                account.Password = SomeTool.HashPassword(register.Password);
                account.Picture = fileBytes;
                await _unit.AccountRepo.AddOneItem(account);

                AccountStatus accountStatus = _mapper.Map<AccountStatus>(register);
                accountStatus.AccountId = id;
                accountStatus.Email = register.Email.Trim();
                accountStatus.IsRole = AccountStatus.Role.Customer;
                await _unit.AccountStatusRepo.AddOneItem(accountStatus);
                return (true, "Account is registed successfully");
            }
            return (false, "Email is existed");
        }

        public async Task<(bool, string)> AddAnAccountForStaff(RegisterForStaffAccountView registerForStaff)
        {
            IEnumerable<Account> checkEmail = await _unit.AccountRepo.GetFieldsByFilterAsync(["Email"],
                    c => c.Email.Equals(registerForStaff.Email.Trim()));
            if (!checkEmail.Any())
            {
                //Encode picture
                byte[] fileBytes;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AccountPictures", "user_default.png");
                using (var ms = new MemoryStream())
                {
                    using (var defaultUserImg = new FileStream(path, FileMode.Open))
                    {
                        await defaultUserImg.CopyToAsync(ms);
                        fileBytes = ms.ToArray();
                    }
                }
                string id = ObjectId.GenerateNewId().ToString();
                Account account = _mapper.Map<Account>(registerForStaff);
                account.AccountId = id;
                account.Email = registerForStaff.Email.Trim();
                account.Password = SomeTool.HashPassword(registerForStaff.Password);
                account.Picture = fileBytes;
                await _unit.AccountRepo.AddOneItem(account);

                AccountStatus accountStatus = _mapper.Map<AccountStatus>(registerForStaff);
                accountStatus.AccountId = id;
                accountStatus.Email = registerForStaff.Email.Trim();
                accountStatus.IsRole = AccountStatus.Role.Staff;
                await _unit.AccountStatusRepo.AddOneItem(accountStatus);
                return (true, "Staff Account is registed successfully");
            }
            return (false, "Email is existed");
        }

        public async Task<(bool, string)> LoginByEmailAndPassword(LoginAccountView login)
        {
            var check = await _unit.AccountRepo.GetFieldsByFilterAsync(["_id"],
                u => u.Email.Equals(login.Email.Trim()) &&
                     u.Password.Equals(SomeTool.HashPassword(login.Password)));
            if (check.Any())
            {
                Account get_id = check.First();
                var get_isBanned = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["IsRole", "IsBanned", "Comments"],
                    u => u.AccountId.Equals(get_id.AccountId));
                AccountStatus accountStatus = get_isBanned.First();
                if (!accountStatus.IsBanned)
                {
                    AuthenticationJwtTool authenticationJwtBearer = new(_configuration);
                    var jwt = authenticationJwtBearer.GenerateJwtToken(accountStatus.AccountId, accountStatus.IsRole.ToString());
                    return (true, jwt);
                }
                return (false, accountStatus.Comments ??= "Thích thì khóa");
            }
            return (false, "Email or Password is invalid");
        }

        public async Task<(bool, string)> GoogleAuthorizeUser(string id_token)
        {
            string accountId;
            AccountStatus.Role role;
            var payload = await GoogleJsonWebSignature.ValidateAsync(id_token);
            var getUserStatus = (await _unit.AccountStatusRepo.GetFieldsByFilterAsync([],
                                g => g.Email.Equals(payload.Email))).FirstOrDefault();
            // nếu chưa có tài khoản thì sẽ đăng ký và đăng nhập
            if (getUserStatus == null)
            {
                string id = ObjectId.GenerateNewId().ToString();
                accountId = id;
                role = AccountStatus.Role.Customer;
                //Encode picture
                byte[] fileBytes;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AccountPictures", "user_default.png");
                using (var ms = new MemoryStream())
                {
                    using (var defaultUserImg = new FileStream(path, FileMode.Open))
                    {
                        await defaultUserImg.CopyToAsync(ms);
                        fileBytes = ms.ToArray();
                    }
                }
                Account account = new()
                {
                    AccountId = id,
                    Password = SomeTool.HashPassword(payload.Email),
                    Email = payload.Email,
                    PhoneNumber = null,
                    Address = null,
                    Picture = fileBytes
                };
                AccountStatus accountStatus = new()
                {
                    AccountId = id,
                    Email = payload.Email,
                    IsAuthenticationEmail = true,
                    IsRole = AccountStatus.Role.Customer,
                    IsBanned = false,
                    Comments = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null
                };
                await _unit.AccountRepo.AddOneItem(account);
                await _unit.AccountStatusRepo.AddOneItem(accountStatus);
            }
            else  // nếu đã có tài khoản thì chỉ đăng nhập
            {
                if (getUserStatus.IsBanned)
                {
                    return (false, getUserStatus.Comments ??= "Thích thì khóa");
                }
                if (!getUserStatus.IsAuthenticationEmail)
                {
                    getUserStatus.IsAuthenticationEmail = true;
                    await _unit.AccountStatusRepo.UpdateItemByValue("AccountId", getUserStatus.AccountId, getUserStatus);
                }
                accountId = getUserStatus.AccountId;
                role = getUserStatus.IsRole;
            }
            AuthenticationJwtTool authenticationJwtBearer = new(_configuration);
            var jwt = authenticationJwtBearer.GenerateJwtToken(accountId, role.ToString());
            return (true, jwt);
        }


        //public async Task<string> RenewToken(string refreshToken, string accessToken)
        //{
        //    AuthenticationJwtTool authenticationJwtBearer = new(_configuration, _unit);
        //    var result = await authenticationJwtBearer.RenewToken(refreshToken, accessToken);
        //    return result;
        //}

        public async Task<(bool, string)> UpdateAnAccount(string id, UpdateAccountView update)
        {
            var check = await _unit.AccountRepo.GetFieldsByFilterAsync([],
                            c => c.AccountId.Equals(id));
            var check_status = await _unit.AccountStatusRepo.GetFieldsByFilterAsync([],
                            c => c.AccountId.Equals(id));

            if (update.PhoneNumber.Length == 10)
            {
                Account account = check.First();
                account.PhoneNumber = update.PhoneNumber;
                account.Address = update.HomeAdress;
                await _unit.AccountRepo.UpdateItemByValue("AccountId", id, account);

                AccountStatus accountStatus = check_status.First();
                accountStatus.UpdatedAt = DateTime.UtcNow;
                await _unit.AccountStatusRepo.UpdateItemByValue("AccountId", id, accountStatus);

                var getContact = (await _unit.ContactRepo.GetFieldsByFilterAsync([],
                    g => g.Email.Equals(account.Email)));
                if (getContact.Any())
                {
                    foreach (var contact in getContact)
                    {
                        contact.Address = update.HomeAdress;
                        contact.Phone = update.PhoneNumber;
                        contact.UpdatedAt = DateTime.UtcNow;
                        await _unit.ContactRepo.UpdateItemByValue("RequestId", contact.RequestId, contact);
                    }
                }
                return (true, "Update Account successfully");
            }
            return (false, "Phone number is not valid");
        }

        public async Task<(bool, string)> SendEmailToResetPassword(string email)
        {
            string resetPasswordLink = $"http://localhost:3000/?reset-password={SomeTool.EncryptEmail(email)}";
            string subject = "Reset Password";
            string body = $"<p>Click here to reset your password:</p>" +
                $"<a href=\"{resetPasswordLink}\" style=\"padding: 10px; color: white; background-color: #007BFF; text-decoration: none;\">Reset password</a>";
            await _emailSender.SendEmailAsync(email, subject, body);
            return (true, "Send Email successfully");
        }

        public async Task<(bool, string)> ResetPassword(ResetPasswordAccountView resetPassword)
        {
            var email = SomeTool.DecryptEmail(resetPassword.Token);
            IEnumerable<Account> check_email = await _unit.AccountRepo.GetFieldsByFilterAsync([],
                            c => c.Email.Equals(email));
            if (check_email.Any())
            {
                Account account = check_email.First();
                account.Password = SomeTool.HashPassword(resetPassword.Password);
                await _unit.AccountRepo.UpdateItemByValue("Email", email, account);

                IEnumerable<AccountStatus> check_isAuthen = await _unit.AccountStatusRepo.GetFieldsByFilterAsync([],
                                c => c.Email.Equals(account.Email));
                AccountStatus accountStatus = check_isAuthen.First();
                accountStatus.IsAuthenticationEmail = true;
                accountStatus.UpdatedAt = System.DateTime.UtcNow;
                await _unit.AccountStatusRepo.UpdateItemByValue("Email", email, accountStatus);
                return (true, "Reset password successfully");
            }
            return (false, "You haven't registed account yet!!!");
        }

        public async Task<(bool, string)> ChangePassword(string id, ChangePasswordAccountView changePassword)
        {
            IEnumerable<Account> check = await _unit.AccountRepo.GetFieldsByFilterAsync([],
                            c => c.AccountId.Equals(id) &&
                                 c.Password.Equals(SomeTool.HashPassword(changePassword.OldPassword)));
            Account account = check.FirstOrDefault()!;
            if (account != null)
            {
                IEnumerable<AccountStatus> check_status = await _unit.AccountStatusRepo.GetFieldsByFilterAsync([],
                            c => c.AccountId.Equals(id));
                AccountStatus accountStatus = check_status.First();
                accountStatus.UpdatedAt = System.DateTime.UtcNow;
                await _unit.AccountStatusRepo.UpdateItemByValue("AccountId", accountStatus.AccountId, accountStatus);

                account.Password = SomeTool.HashPassword(changePassword.Password);
                await _unit.AccountRepo.UpdateItemByValue("AccountId", account.AccountId, account);
                return (true, "Change password successfully");
            }
            return (false, "Invalid old password");
        }

        public async Task<(bool, string)> BanAnAccount(BanAccountView ban)
        {
            IEnumerable<AccountStatus> check_email = await _unit.AccountStatusRepo.GetFieldsByFilterAsync([],
                    c => c.Email.Equals(ban.Email.Trim()));
            if (check_email.Any())
            {
                AccountStatus accountStatus = check_email.First();
                accountStatus.IsBanned = true;
                accountStatus.Comments = ban.Comments;
                await _unit.AccountStatusRepo.UpdateItemByValue("Email", accountStatus.Email, accountStatus);
                return (true, "Ban account successfully");
            }
            return (false, "Unexisted Email");
        }

        public async Task<(bool, string)> DeleteAnAccount(DeleteAccountView delete)
        {
            IEnumerable<AccountStatus> check_emailStatus = await _unit.AccountStatusRepo.GetFieldsByFilterAsync([],
                    c => c.Email.Equals(delete.Email.Trim()));
            if (check_emailStatus.Any())
            {
                await _unit.AccountRepo.RemoveItemByValue("Email", delete.Email.Trim());
                await _unit.AccountStatusRepo.RemoveItemByValue("Email", delete.Email.Trim());
                string subject = "Notice";
                string body = $"<h3><strong>Your SWD Account haven't been deleted by {delete.Comments}</strong></h3>";
                await _emailSender.SendEmailAsync(delete.Email, subject, body);
                return (true, "Remove account successfully");
            }
            return (false, "Unexisted Email");
        }


        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<(bool, object)> ViewProfile(string email)
        {
            IEnumerable<Account> getUser = await _unit.AccountRepo.GetFieldsByFilterAsync([],
                            g => g.Email.Equals(email));
            IEnumerable<AccountStatus> getUserStatus = await _unit.AccountStatusRepo.GetFieldsByFilterAsync([],
                            g => g.Email.Equals(email));
            if (getUser.Any() && getUserStatus.Any())
            {
                var getProfileAccount = getUser.First();
                getProfileAccount.Picture = SomeTool.GetImage(Convert.ToBase64String(getProfileAccount.Picture))!;
                var getProfileStatus = getUserStatus.First();
                var reponse = new
                {
                    Email = getProfileAccount.Email,
                    PhoneNumber = getProfileAccount.PhoneNumber,
                    Address = getProfileAccount.Address,
                    Picture = getProfileAccount.Picture,
                    CreatedAt = getProfileStatus.CreatedAt,
                    UpdatedAt = getProfileStatus.UpdatedAt
                };
                return (true, reponse);
            }
            return (false, "null");
        }

        public async Task UpdatePictureAccount(string id, UpdatePictureAccountView updatePicture)
        {
            IEnumerable<Account> getUser = await _unit.AccountRepo.GetFieldsByFilterAsync([],
                            g => g.AccountId.Equals(id));
            if (getUser.Any())
            {
                var getFieldsUser = getUser.First();
                if (updatePicture.Picture.Length > 0)
                {
                    //Encode picture
                    byte[] fileBytes;
                    using (var ms = new MemoryStream())
                    {
                        await updatePicture.Picture.CopyToAsync(ms);
                        fileBytes = ms.ToArray();
                    }
                    IEnumerable<AccountStatus> getUserStatus = await _unit.AccountStatusRepo.GetFieldsByFilterAsync([],
                            c => c.AccountId.Equals(id));
                    var accountStatus = getUserStatus.First();
                    getFieldsUser.Picture = fileBytes;
                    accountStatus.UpdatedAt = System.DateTime.UtcNow;
                    await _unit.AccountRepo.UpdateItemByValue("AccountId", getFieldsUser.AccountId, getFieldsUser);
                    await _unit.AccountStatusRepo.UpdateItemByValue("AccountId", getFieldsUser.AccountId, accountStatus);
                }
            }
        }

        public async Task<object> GetPagingAccount(PagingAccountView paging)
        {
            const int pageSize = 5;
            const string sortField = "Email";
            List<string> searchFields = ["Email", "PhoneNumber"];
            List<string> returnFields = ["Email", "PhoneNumber"];

            int skip = (paging.PageIndex - 1) * pageSize;
            var items = (await _unit.AccountRepo.PagingAsync(skip, pageSize, paging.IsAsc, sortField, paging.SearchValue, searchFields, returnFields)).ToList();

            var responses = new List<object>();

            foreach (var item in items)
            {
                responses.Add(new
                {
                    Email = item.Email,
                    Phone = item.PhoneNumber
                });
            }
            return responses;
        }


        public async Task<(bool, object)> GetAccountDetail(DetailAccountView detail)
        {
            var getUserAccount = (await _unit.AccountRepo.GetFieldsByFilterAsync([],
                             g => g.Email.Equals(detail.Email))).FirstOrDefault();

            var getUserStatus = (await _unit.AccountStatusRepo.GetFieldsByFilterAsync([],
                            g => g.Email.Equals(detail.Email))).FirstOrDefault();
            if (getUserAccount != null && getUserStatus != null)
            {
                getUserAccount.Picture = SomeTool.GetImage(Convert.ToBase64String(getUserAccount.Picture))!;
                var response = new
                {
                    Email = getUserAccount.Email,
                    Phone = getUserAccount.PhoneNumber,
                    Address = getUserAccount.Address,
                    Picture = getUserAccount.Picture,
                    IsAuthenticationEmail = getUserStatus.IsAuthenticationEmail,
                    Role = getUserStatus.IsRole,
                    IsBanned = getUserStatus.IsBanned,
                    Comments = getUserStatus.Comments
                };
                return (true, response);
            }
            return (false, "Unexisted Email");
        }

    }
}
