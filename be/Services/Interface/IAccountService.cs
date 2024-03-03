using Microsoft.AspNetCore.Http;
using Repositories.Model;
using Repositories.Models;
using Repositories.ModelView;
using static Repositories.ModelView.AccountView;

namespace Services.Interface
{
    public interface IAccountService
    {
        Task<string> AddAnAccountForCustomer(RegisterAccountView register);
        Task<string> AddAnAccountForStaff(RegisterForStaffAccountView registerForStaff);
        Task<string> LoginByEmailAndPassword(LoginAccountView login);
        Task<string> GoogleAuthorizeUser(string id_token);
        //Task<string> RenewToken(string refreshToken, string accessToken);
        Task<string> UpdateAnAccount(string id, UpdateAccountView update);
        Task UpdatePictureAccount(string id, UpdatePictureAccountView updatePicture);
        Task<string> SendEmailToResetPassword(string email);
        Task<string> ResetPassword(ResetPasswordAccountView resetPasswordAccountView);
        Task<string> ChangePassword(string id, ChangePasswordAccountView changePassword);
        Task<string> BanAnAccount(BanAccountView ban);
        Task<string> DeleteAnAccount(DeleteAccountView delete);
        Task<object> ViewProfile(string email);
        Task<object> GetPagingAccount(PagingAccountView paging);
        Task<object> GetAccountDetail(DetailAccountView detail);
        Task SignOutAsync();
    }
}
