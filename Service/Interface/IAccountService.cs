using static Repository.ModelView.AccountView;

namespace Service.Interface
{
    public interface IAccountService
    {
        Task<string> AddAnAccountForCustomer(RegisterAccountView register);
        Task<string> AddAnAccountForStaff(RegisterForStaffAccountView registerForStaff);
        Task<string> LoginByUsernameAndPassword(LoginAccountView login);
        Task<string> UpdateAnAccount(UpdateAccountView update);
        Task<string> SendEmailToResetPassword(string email);
        Task<string> ResetPassword(ResetPasswordAccountView resetPasswordAccountView);
        Task<string> ChangePassword(ChangePasswordAccountView changePassword);
        Task<string> BanAnAccount(BanAccountView ban);
        Task<string> DeleteAnAccount(DeleteAccountView delete);
    }
}
