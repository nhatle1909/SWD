using Microsoft.AspNetCore.Http;
using Repositories.Model;
using Repositories.Models;
using Repositories.ModelView;
using static Repositories.ModelView.AccountView;

namespace Services.Interface
{
    public interface IAccountService
    {
        Task<(bool, string)> AddAnAccountForCustomer(RegisterAccountView register);
        Task<(bool, string)> AddAnAccountForStaff(RegisterForStaffAccountView registerForStaff);
        Task<(bool, string)> LoginByEmailAndPassword(LoginAccountView login);
        Task<(bool, string)> GoogleAuthorizeUser(string id_token);
        //Task<string> RenewToken(string refreshToken, string accessToken);
        Task<(bool, string)> UpdateAnAccount(string id, UpdateAccountView update);
        Task UpdatePictureAccount(string id, UpdatePictureAccountView updatePicture);
        Task<(bool, string)> SendEmailToResetPassword(string email);
        Task<(bool, string)> ResetPassword(ResetPasswordAccountView resetPasswordAccountView);
        Task<(bool, string)> ChangePassword(string id, ChangePasswordAccountView changePassword);
        Task<(bool, string)> BanAnAccount(BanAccountView ban);
        Task<(bool, string)> DeleteAnAccount(DeleteAccountView delete);
        Task<(bool, object)> ViewProfile(string email);
        Task<object> GetPagingAccount(PagingAccountView paging);
        Task<(bool, object)> GetAccountDetail(DetailAccountView detail);
        Task SignOutAsync();
    }
}
