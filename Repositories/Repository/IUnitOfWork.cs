using MongoDB.Driver;
using Repository.Model;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public interface IUnitOfWork
    {
        IRepository<Account> AccountRepo { get; }
        IRepository<AccountStatus> AccountStatusRepo {  get; }
        IRepository<Material> MaterialRepo { get; }
        IRepository<Interior> InteriorRepo { get; }
        IRepository<Blog> BlogRepo { get; }
    }
}
