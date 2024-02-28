using MongoDB.Driver;
using Repositories.Model;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public interface IUnitOfWork
    {
        IRepository<Account> AccountRepo { get; }
        IRepository<AccountStatus> AccountStatusRepo {  get; }
        IRepository<Material> MaterialRepo { get; }
        IRepository<Interior> InteriorRepo { get; }
        IRepository<Blog> BlogRepo { get; }
        IRepository<BlogComment> BlogCommentRepo { get; }
        IRepository<Contact> ContactRepo { get; }
    }
}
