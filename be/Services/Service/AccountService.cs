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

        public async Task<string> AddAnAccountForCustomer(RegisterAccountView register)
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
                return "Account is registed successfully";
            }
            return "Email is existed";
        }

        public async Task<string> AddAnAccountForStaff(RegisterForStaffAccountView registerForStaff)
        {
            string _id = AuthenticationJwtTool.GetUserIdFromJwt(registerForStaff.Jwt);
            IEnumerable<AccountStatus> check = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["_id", "IsRole"],
                            c => c.AccountId.Equals(_id));
            if (check.Any())
            {
                AccountStatus role = check.First();
                if (role.IsRole == AccountStatus.Role.Admin)
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
                        return "Staff Account is registed successfully";
                    }
                    return "Email is existed";
                }
                return "You have not permission to use this function";
            }
            return "Account is not existed";
        }



        public async Task<string> LoginByEmailAndPassword(LoginAccountView login)
        {
            IEnumerable<Account> check = await _unit.AccountRepo.GetFieldsByFilterAsync(["_id"],
                u => u.Email.Equals(login.Email.Trim()) &&
                     u.Password.Equals(SomeTool.HashPassword(login.Password)));
            if (check.Any())
            {
                Account get_id = check.First();
                IEnumerable<AccountStatus> get_isBanned = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["IsBanned", "Comments"],
                    u => u.AccountId.Equals(get_id.AccountId));
                AccountStatus accountStatus = get_isBanned.First();
                if (!accountStatus.IsBanned)
                {
                    AuthenticationJwtTool authenticationJwtBearer = new(_configuration);
                    string jwt = authenticationJwtBearer.GenerateJwtToken(accountStatus.AccountId, 1);
                    return jwt;
                }
                return accountStatus.Comments ??= "Thích thì khóa";
            }
            return "Email or Password is invalid";
        }
        public async Task<string> UpdateAnAccount(UpdateAccountView update)
        {
            string _id = AuthenticationJwtTool.GetUserIdFromJwt(update.Jwt);
            IEnumerable<Account> check = await _unit.AccountRepo.GetFieldsByFilterAsync([],
                            c => c.AccountId.Equals(_id));
            IEnumerable<AccountStatus> check_status = await _unit.AccountStatusRepo.GetFieldsByFilterAsync([],
                            c => c.AccountId.Equals(_id));
            if (check.Any())
            {
                Account account = check.First();
                IEnumerable<AccountStatus> check_email = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["Email"],
                            c => c.Email.Equals(update.Email.Trim()));
                if (account.Email.Equals(update.Email.Trim()) || !check_email.Any())
                {
                    if (update.PhoneNumber == null || update.PhoneNumber.Length == 10)
                    {
                        account.Email = update.Email.Trim();
                        account.PhoneNumber = update.PhoneNumber;
                        account.Address = update.HomeAdress;
                        await _unit.AccountRepo.UpdateItemByValue("AccountId", _id, account);

                        AccountStatus accountStatus = check_status.First();
                        accountStatus.Email = update.Email.Trim();
                        accountStatus.UpdatedAt = DateTime.Now;
                        await _unit.AccountStatusRepo.UpdateItemByValue("AccountId", _id, accountStatus);
                        return "Update Account successfully";
                    }
                    return "Phone number is not valid";
                }
                return "Email is existed";
            }
            return "Account is not existed";
        }

        public async Task<string> SendEmailToResetPassword(string email)
        {
            string resetPasswordLink = "http://127.0.0.1:5500/ResetPassword.html?email=" + HttpUtility.UrlEncode(email);
            string subject = "Reset Password";
            string body = $"<p>Click here to reset your password:</p>" +
                $"<a href=\"{resetPasswordLink}\" style=\"padding: 10px; color: white; background-color: #007BFF; text-decoration: none;\">Reset password</a>";
            await _emailSender.SendEmailAsync(email, subject, body);
            return "Send Email successfully";
        }

        public async Task<string> ResetPassword(ResetPasswordAccountView resetPassword)
        {
            IEnumerable<Account> check_email = await _unit.AccountRepo.GetFieldsByFilterAsync([],
                            c => c.Email.Equals(resetPassword.Email.Trim()));
            IEnumerable<AccountStatus> check_isAuthen = await _unit.AccountStatusRepo.GetFieldsByFilterAsync([],
                            c => c.Email.Equals(resetPassword.Email.Trim()));
            if (check_email.Any())
            {
                Account account = check_email.First();
                account.Password = SomeTool.HashPassword(resetPassword.Password);
                await _unit.AccountRepo.UpdateItemByValue("Email", resetPassword.Email, account);

                AccountStatus accountStatus = check_isAuthen.First();
                accountStatus.IsAuthenticationEmail = true;
                accountStatus.UpdatedAt = System.DateTime.Now;
                await _unit.AccountStatusRepo.UpdateItemByValue("Email", resetPassword.Email, accountStatus);
                return "Reset password successfully";
            }
            return "You haven't registed account yet!!!";
        }

        public async Task<string> ChangePassword(ChangePasswordAccountView changePassword)
        {
            string _id = AuthenticationJwtTool.GetUserIdFromJwt(changePassword.Jwt);
            IEnumerable<Account> check = await _unit.AccountRepo.GetFieldsByFilterAsync([],
                            c => c.AccountId.Equals(_id) &&
                                 c.Password.Equals(SomeTool.HashPassword(changePassword.OldPassword)));
            Account account = check.FirstOrDefault()!;
            if (account != null)
            {
                IEnumerable<AccountStatus> check_status = await _unit.AccountStatusRepo.GetFieldsByFilterAsync([],
                            c => c.AccountId.Equals(_id));
                AccountStatus accountStatus = check_status.First();
                accountStatus.UpdatedAt = System.DateTime.Now;
                await _unit.AccountStatusRepo.UpdateItemByValue("AccountId", accountStatus.AccountId, accountStatus);

                account.Password = SomeTool.HashPassword(changePassword.Password);
                await _unit.AccountRepo.UpdateItemByValue("AccountId", account.AccountId, account);
                return "Change password successfully";
            }
            return "Invalid old password";
        }

        public async Task<string> BanAnAccount(BanAccountView ban)
        {
            string _id = AuthenticationJwtTool.GetUserIdFromJwt(ban.Jwt);
            IEnumerable<AccountStatus> check = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["_id", "IsRole"],
                            c => c.AccountId.Equals(_id));
            if (check.Any())
            {
                AccountStatus role = check.First();
                if (role.IsRole == AccountStatus.Role.Admin)
                {
                    IEnumerable<AccountStatus> check_email = await _unit.AccountStatusRepo.GetFieldsByFilterAsync([],
                            c => c.Email.Equals(ban.Email.Trim()));
                    if (check_email.Any())
                    {
                        AccountStatus accountStatus = check_email.First();
                        accountStatus.IsBanned = true;
                        accountStatus.Comments = ban.Comments;
                        await _unit.AccountStatusRepo.UpdateItemByValue("Email", accountStatus.Email, accountStatus);
                        return "Ban account successfully";
                    }
                    return "Unexisted Email";
                }
                return "You have not permission to use this function";
            }
            return "Account is not existed";
        }

        public async Task<string> DeleteAnAccount(DeleteAccountView delete)
        {
            string _id = AuthenticationJwtTool.GetUserIdFromJwt(delete.Jwt);
            IEnumerable<AccountStatus> check = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["_id", "IsRole"],
                            c => c.AccountId.Equals(_id));
            if (check.Any())
            {
                AccountStatus role = check.First();
                if (role.IsRole == AccountStatus.Role.Admin)
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
                        return "Remove account successfully";
                    }
                    return "Unexisted Email";
                }
                return "You have not permission to use this function";
            }
            return "Account is not existed";
        }


        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<object> ViewProfile(string email)
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
                return reponse;
            }
            return "null";
        }

        public async Task UpdatePictureAccount(UpdatePictureAccountView updatePicture)
        {
            string _id = AuthenticationJwtTool.GetUserIdFromJwt(updatePicture.Jwt);
            IEnumerable<Account> getUser = await _unit.AccountRepo.GetFieldsByFilterAsync([],
                            g => g.AccountId.Equals(_id));
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
                            c => c.AccountId.Equals(_id));
                    var accountStatus = getUserStatus.First();
                    getFieldsUser.Picture = fileBytes;
                    accountStatus.UpdatedAt = System.DateTime.Now;
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

            string _id = AuthenticationJwtTool.GetUserIdFromJwt(paging.Jwt);
            var getUserStatus = (await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["_id", "IsRole"],
                            g => g.AccountId.Equals(_id))).FirstOrDefault();
            if (getUserStatus != null)
            {
                if (getUserStatus.IsRole == AccountStatus.Role.Admin)
                {
                    int skip = (paging.PageIndex - 1) * pageSize;
                    var items = (await _unit.AccountRepo.PagingAsync(skip, pageSize, paging.IsAsc, sortField, paging.SearchValue, searchFields, returnFields)).ToList();
                    return items;
                }
                return "You have not permission to use this function";
            }
            return "Account is not existed";
        }

        public async Task<object> GetAccountDetail(DetailAccountView detail)
        {
            string _id = AuthenticationJwtTool.GetUserIdFromJwt(detail.Jwt);
            var getUserAccount = await _unit.AccountRepo.GetFieldsByFilterAsync([],
                             g => g.AccountId.Equals(_id) || 
                                  g.Email.Equals(detail.Email));

            var getUserStatus = await _unit.AccountStatusRepo.GetFieldsByFilterAsync([],
                            g => g.AccountId.Equals(_id) ||
                                 g.Email.Equals(detail.Email));
            var getFirstUserStatus = getUserStatus.FirstOrDefault();
            if (getFirstUserStatus != null)
            {
                if (getFirstUserStatus.IsRole == AccountStatus.Role.Admin)
                {
                    var getSecondUser = getUserAccount.ElementAt(1);
                    var getSecondStatus = getUserStatus.ElementAt(1);
                    var response = new
                    {
                        Email = getSecondUser.Email,
                        Phone = getSecondUser.PhoneNumber,
                        Address = getSecondUser.Address,
                        Picture = getSecondUser.Picture,
                        IsAuthenticationEmail = getSecondStatus.IsAuthenticationEmail,
                        Role = getSecondStatus.IsRole,
                        IsBanned = getSecondStatus.IsBanned,
                        Comments = getSecondStatus.Comments
                    };
                    return response;
                }
                return "You have not permission to use this function";
            }
            return "Account is not existed";
        }
    }
}
