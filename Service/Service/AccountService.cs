using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Repository.Model;
using Repository.Models;
using Repository.Repository;
using Repository.Tools;
using Service.Interface;
using System.Web;
using static Repository.ModelView.AccountView;

namespace Service.Service
{

    public class AccountService : IAccountService
    {
        private readonly IRepository<Account> _repoAccount;
        private readonly IRepository<AccountStatus> _repoAccountStatus;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountService> _logger;
        private readonly IEmailSender _emailSender;
        public AccountService(IRepository<Account> repoAccount, IRepository<AccountStatus> repoAccountStatus, IConfiguration configuration, IMapper mapper, ILogger<AccountService> logger, IEmailSender emailSender)
        {
            _repoAccount = repoAccount;
            _repoAccountStatus = repoAccountStatus;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
            _emailSender = emailSender;
        }

        public async Task<string> AddAnAccountForCustomer(RegisterAccountView register)
        {
            IEnumerable<Account> check = await _repoAccount.GetFieldsByFilterAsync(["Email"],
                            c => c.Email.Equals(register.Email.Trim()));
            if (!check.Any())
            {
                string id = ObjectId.GenerateNewId().ToString();
                Account account = _mapper.Map<Account>(register);
                account.AccountId = id;
                account.Email = register.Email.Trim();
                account.Password = IdGenerator.HashPassword(register.Password);
                await _repoAccount.AddOneItem(account);

                AccountStatus accountStatus = _mapper.Map<AccountStatus>(register);
                accountStatus._id = id;
                accountStatus.Email = register.Email.Trim();
                accountStatus.IsRole = Account.Role.Customer;
                await _repoAccountStatus.AddOneItem(accountStatus);
                return "Account is registed successfully";
            }
            return "Email is existed";
        }

        public async Task<string> AddAnAccountForStaff(RegisterForStaffAccountView registerForStaff)
        {
            IEnumerable<AccountStatus> check = await _repoAccountStatus.GetFieldsByFilterAsync(["_id", "IsRole"],
                            c => c._id.Equals(registerForStaff._id));
            if (check.Any())
            {
                AccountStatus role = check.First();
                if (role.IsRole == Account.Role.Admin)
                {
                    IEnumerable<Account> checkEmail = await _repoAccount.GetFieldsByFilterAsync(["Email"],
                            c => c.Email.Equals(registerForStaff.Email.Trim()));
                    if (!checkEmail.Any())
                    {
                        string id = ObjectId.GenerateNewId().ToString();
                        Account account = _mapper.Map<Account>(registerForStaff);
                        account.AccountId = id;
                        account.Email = registerForStaff.Email.Trim();
                        account.Password = IdGenerator.HashPassword(registerForStaff.Password);
                        await _repoAccount.AddOneItem(account);

                        AccountStatus accountStatus = _mapper.Map<AccountStatus>(registerForStaff);
                        accountStatus._id = id;
                        accountStatus.Email = registerForStaff.Email.Trim();
                        accountStatus.IsRole = Account.Role.Staff;
                        await _repoAccountStatus.AddOneItem(accountStatus);
                        return "Staff Account is registed successfully";
                    }
                    return "Email is existed";
                }
                return "You have not permission to use this function";
            }
            return "Account is not existed";
        }



        public async Task<string> LoginByUsernameAndPassword(LoginAccountView login)
        {
            IEnumerable<Account> check = await _repoAccount.GetFieldsByFilterAsync(["_id"],
                u => u.Email.Equals(login.Email.Trim()) &&
                     u.Password.Equals(IdGenerator.HashPassword(login.Password)));
            if (check.Any())
            {
                Account get_id = check.First();
                IEnumerable<AccountStatus> get_isBanned = await _repoAccountStatus.GetFieldsByFilterAsync(["IsBanned", "Comments"],
                    u => u._id.Equals(get_id.AccountId));
                AccountStatus accountStatus = get_isBanned.First();
                if (!accountStatus.IsBanned)
                {
                    Authentication authenticationJwtBearer = new(_configuration);
                    string jwt = authenticationJwtBearer.GenerateJwtToken(accountStatus._id, accountStatus.IsRole.ToString(), 1);
                    return jwt;
                }
                return accountStatus.Comments ??= "Thích thì khóa";
            }
            return "Email or Password is invalid";
        }

        public async Task<string> UpdateAnAccount(UpdateAccountView update)
        {
            IEnumerable<Account> check = await _repoAccount.GetFieldsByFilterAsync([],
                            c => c.AccountId.Equals(update._id));
            IEnumerable<AccountStatus> check_status = await _repoAccountStatus.GetFieldsByFilterAsync([],
                            c => c._id.Equals(update._id));
            if (check.Any())
            {
                Account account = check.First();
                IEnumerable<AccountStatus> check_email = await _repoAccountStatus.GetFieldsByFilterAsync(["Email"],
                            c => c.Email.Equals(update.Email.Trim()));
                if (account.Email.Equals(update.Email.Trim()) || !check_email.Any())
                {
                    if (update.PhoneNumber == null || update.PhoneNumber.Length == 10)
                    {
                        account.Email = update.Email.Trim();
                        account.PhoneNumber = update.PhoneNumber;
                        account.Address = update.HomeAdress;
                        await _repoAccount.UpdateItemByValue(update._id, account);

                        AccountStatus accountStatus = check_status.First();
                        accountStatus.Email = update.Email.Trim();
                        accountStatus.UpdatedAt = DateTime.Now;
                        await _repoAccountStatus.UpdateItemByValue(update._id, accountStatus);
                        return "Update successfully";
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
            IEnumerable<Account> check_email = await _repoAccount.GetFieldsByFilterAsync([],
                            c => c.Email.Equals(resetPassword.Email.Trim()));
            IEnumerable<AccountStatus> check_isAuthen = await _repoAccountStatus.GetFieldsByFilterAsync([],
                            c => c.Email.Equals(resetPassword.Email.Trim()));
            if (check_email.Any())
            {
                Account account = check_email.First();
                account.Password = IdGenerator.HashPassword(resetPassword.Password);
                await _repoAccount.UpdateItemByValue(resetPassword.Email, account);

                AccountStatus accountStatus = check_isAuthen.First();
                accountStatus.IsAuthenticationEmail = true;
                accountStatus.UpdatedAt = DateTime.Now;
                await _repoAccountStatus.UpdateItemByValue(resetPassword.Email, accountStatus);
                return "Reset password successfully";
            }
            return "You haven't registed account yet!!!";
        }

        public async Task<string> ChangePassword(ChangePasswordAccountView changePassword)
        {
            IEnumerable<Account> check = await _repoAccount.GetFieldsByFilterAsync([],
                            c => c.AccountId.Equals(changePassword._id) &&
                                 c.Password.Equals(IdGenerator.HashPassword(changePassword.OldPassword)));
            Account account = check.FirstOrDefault()!;
            if (account != null)
            {
                IEnumerable<AccountStatus> check_status = await _repoAccountStatus.GetFieldsByFilterAsync([],
                            c => c._id.Equals(changePassword._id));
                AccountStatus accountStatus = check_status.First();
                accountStatus.UpdatedAt = DateTime.Now;
                await _repoAccountStatus.UpdateItemByValue(accountStatus._id, accountStatus);

                account.Password = IdGenerator.HashPassword(changePassword.Password);
                await _repoAccount.UpdateItemByValue(account.AccountId, account);
                return "Change password successfully";
            }
            return "Invalid old password";
        }

        public async Task<string> BanAnAccount(BanAccountView ban)
        {
            IEnumerable<AccountStatus> check = await _repoAccountStatus.GetFieldsByFilterAsync(["_id", "IsRole"],
                            c => c._id.Equals(ban._id));
            if (check.Any())
            {
                AccountStatus role = check.First();
                if (role.IsRole == Account.Role.Admin)
                {
                    IEnumerable<AccountStatus> check_email = await _repoAccountStatus.GetFieldsByFilterAsync([],
                            c => c.Email.Equals(ban.Email.Trim()));
                    if (check_email.Any())
                    {
                        AccountStatus accountStatus = check_email.First();
                        accountStatus.IsBanned = true;
                        accountStatus.Comments = ban.Comments;
                        await _repoAccountStatus.UpdateItemByValue(accountStatus.Email, accountStatus);
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
            IEnumerable<AccountStatus> check = await _repoAccountStatus.GetFieldsByFilterAsync(["_id", "IsRole"],
                            c => c._id.Equals(delete._id));
            if (check.Any())
            {
                AccountStatus role = check.First();
                if (role.IsRole == Account.Role.Admin)
                {
                    IEnumerable<AccountStatus> check_emailStatus = await _repoAccountStatus.GetFieldsByFilterAsync([],
                            c => c.Email.Equals(delete.Email.Trim()));
                    if (check_emailStatus.Any())
                    {
                        await _repoAccount.RemoveItemByValue(delete.Email.Trim());
                        await _repoAccountStatus.RemoveItemByValue(delete.Email.Trim());
                        string subject = "Notice";
                        string body = $"<h3><strong>Your KidGo Account haven't been deleted by {delete.Comments}</strong></h3>";
                        await _emailSender.SendEmailAsync(delete.Email, subject, body);
                        return "Remove account successfully";
                    }
                    return "Unexisted Email";
                }
                return "You have not permission to use this function";
            }
            return "Account is not existed";
        }
    }
}
