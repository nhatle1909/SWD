using AutoMapper;
using EXE.Interface;
using EXE.Tools;
using Models.Repository;
using Models.Model;
using Models.ModelView;

namespace EXE.Service
{

    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Account> _repos;
        private readonly IConfiguration _configuration;
        public AccountService(IRepository<Account> AccountRepo, IConfiguration configuration, IMapper mapper)
        {

            _repos = AccountRepo;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<Account> AddOneAccount(AccountView accountView)
        {
            Account account = _mapper.Map<Account>(accountView);
            account.AccountId = IdGenerator.GenerateID();
            account.Password = IdGenerator.GenerateID();
            account.IsBanned = false;
            return await _repos.AddOneItem(account);
        }

        public async Task<Account> BannedOneAccount(string id, AccountView accountView)
        {
            IEnumerable<Account> Selectedaccount = await _repos.GetByFilterAsync(a => a.AccountId.Equals(id));

            if (!Selectedaccount.Any())
            {
                throw new Exception($"Account with id {id} not found");
            }
            Account account = _mapper.Map<Account>(accountView);
            account.AccountId = id;
            account.IsBanned = true;
            return await _repos.UpdateItemByValue(id, account);

        }

        public async Task<(string, AccountView)> Login(string Username, string Password)
        {
            IEnumerable<Account> credential = await _repos.GetByFilterAsync(
                a => a.Username.Equals(Username) &&
                a.Password.Equals(Password)
                );
            if (!credential.Any())
            {
                throw new Exception("Invalid Credential");
            }
            Account account = credential.First();

            if (account.IsBanned) throw new Exception("Account is Banned");
            AccountView accountView = _mapper.Map<AccountView>(account);
            Authentication authentication = new(_configuration);
            string token = authentication.GenerateJwtToken(account.AccountId, 1);
            return (token, accountView);

        }

        public Task<Account> Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<Account> UpdateAccount(string id, AccountView accountView)
        {
            IEnumerable<Account> Selectedaccount = await _repos.GetByFilterAsync(a => a.AccountId.Equals(id));

            if (!Selectedaccount.Any())
            {
                throw new Exception($"Account with id {id} not found");
            }
            Account account = _mapper.Map<Account>(accountView);
            account.AccountId = id;
            return await _repos.UpdateItemByValue(id, account);

        }
    }
}
