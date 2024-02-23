using Microsoft.AspNetCore.Http;
using Repository.Model;
using Repository.Models;
using static Repository.ModelView.AccountView;

namespace Service.Interface
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
        Task<(Account?, AccountStatus?)> ViewProfile(ViewProfileAccountView viewProfile);
        Task SignOutAsync();
    }
}
