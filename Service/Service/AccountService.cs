using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Repository.Model;
using Repository.Models;
using Repository.ModelView;
using Repository.Repository;
using Repository.Tools;
using Service.Interface;

namespace Service.Service
{

    public class AccountService : IAccountService
    {
        private readonly IRepository<Account> _repoAccount;
        private readonly IRepository<AccountStatus> _repoAccountStatus;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountService> _logger;
        public AccountService(IRepository<Account> repoAccount, IRepository<AccountStatus> repoAccountStatus, IConfiguration configuration, IMapper mapper, ILogger<AccountService> logger)
        {
            _repoAccount = repoAccount;
            _repoAccountStatus = repoAccountStatus;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;

        }

        public async Task<Account> AddOneAccount(AccountView accountView)
        {
            IEnumerable<Account> checkExist = await _repoAccount.GetByFilterAsync(a => a.Email.Equals(accountView.Email));
            if (!checkExist.Any())
            {
                Account account = _mapper.Map<Account>(accountView);

                account.Email = accountView.Email.Trim();
                account.AccountId = ObjectId.GenerateNewId().ToString();
                account.Password = IdGenerator.HashPassword(accountView.Password);



                AccountStatus accountStatus = _mapper.Map<AccountStatus>(accountView);
                accountStatus._id = account.AccountId;
                accountStatus.Email = accountView.Email.Trim();
                accountStatus.IsRole = Account.Role.Customer;
                await _repoAccountStatus.AddOneItem(accountStatus);
                return await _repoAccount.AddOneItem(account); ;
            }
            else
            {
                throw new Exception($" {accountView.Email} existed");
            }
        }

        public async Task<Account> BannedOneAccount(string id, AccountView accountView)
        {
            IEnumerable<Account> Selectedaccount = await _repoAccount.GetByFilterAsync(a => a.AccountId.Equals(id));

            if (!Selectedaccount.Any())
            {
                throw new Exception($"Account with id {id} not found");
            }
            Account account = _mapper.Map<Account>(accountView);
            account.AccountId = id;

            return await _repoAccount.UpdateItemByValue(id, account);

        }

        public async Task<(string, AccountView)> Login(string Username, string Password)
        {
            IEnumerable<Account> credential = await _repoAccount.GetByFilterAsync(
                a => a.Email.Equals(Username) &&
                a.Password.Equals(Password)
                );

            if (!credential.Any())
            {
                throw new Exception("Invalid Credential");
            }


            Account account = credential.First();

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
            IEnumerable<Account> Selectedaccount = await _repoAccount.GetByFilterAsync(a => a.AccountId.Equals(id));

            if (!Selectedaccount.Any())
            {
                throw new Exception($"Account with id {id} not found");
            }
            Account account = _mapper.Map<Account>(accountView);
            account.AccountId = id;
            return await _repoAccount.UpdateItemByValue(id, account);

        }
    }
}
