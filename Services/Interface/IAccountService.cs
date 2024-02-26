using Microsoft.AspNetCore.Http;
using Repositories.Model;
using Repositories.Models;
using static Repositories.ModelView.AccountView;

namespace Services.Interface
{
    public interface IAccountService
    {
        Task<string> AddAnAccountForCustomer(RegisterAccountView register);
        Task<string> AddAnAccountForStaff(RegisterForStaffAccountView registerForStaff);
        Task<string> LoginByEmailAndPassword(LoginAccountView login);
        Task<string> UpdateAnAccount(UpdateAccountView update);
        Task UpdatePictureAccount(UpdatePictureAccountView updatePicture);
        Task<string> SendEmailToResetPassword(string email);
        Task<string> ResetPassword(ResetPasswordAccountView resetPasswordAccountView);
        Task<string> ChangePassword(ChangePasswordAccountView changePassword);
        Task<string> BanAnAccount(BanAccountView ban);
        Task<string> DeleteAnAccount(DeleteAccountView delete);
        Task<object> ViewProfile(string email);
        Task<object> GetPagingAccount(PagingAccountView paging);
        Task<object> GetAccountDetail(DetailAccountView detail);
        Task SignOutAsync();
    }
}
