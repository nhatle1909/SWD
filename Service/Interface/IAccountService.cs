
using Repository.Model;
using Repository.ModelView;

namespace Service.Interface
{
    public interface IAccountService
    {
        //public Task<IEnumerable<AccountModelView>> GetAllTemplateItem();
        //public Task<AccountModelView> AddOneTemplateItem(TemplateModelView TemplateModelView);
        //public Task<AccountModelView    > UpdateTemplateItem(string id, TemplateModelView TemplateModelView);
        //public Task<bool> DeleteTemplateItem(string id);

        public Task<(string, AccountView)> Login(string Username, string Password);
        public Task<Account> Logout();
        public Task<Account> AddOneAccount(AccountView accountView);
        public Task<Account> UpdateAccount(string id, AccountView accountView);
        public Task<Account> BannedOneAccount(string id, AccountView accountView);
    }
}
