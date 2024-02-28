using Models.Repository;
using MongoDB.Driver;
using Repositories.Model;
using Repositories.Models;
using Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;          
        }

        private readonly IMongoClient _mongoClient;

        private IRepository<Account> _accountRepo;
        public IRepository<Account> AccountRepo
        {
            get
            {
                if (_accountRepo is null)
                    _accountRepo = new Repository<Account>(_mongoClient);
                return _accountRepo;
            }
        }

        private IRepository<AccountStatus> _accountStatusRepo;
        public IRepository<AccountStatus> AccountStatusRepo
        {
            get
            {
                if (_accountStatusRepo is null)
                    _accountStatusRepo = new Repository<AccountStatus>(_mongoClient);
                return _accountStatusRepo;
            }
        }


        private IRepository<Material> _materialRepo;
        public IRepository<Material> MaterialRepo
        {
            get
            {
                if (_materialRepo is null)
                    _materialRepo = new Repository<Material>(_mongoClient);
                return _materialRepo;
            }
        }


        private IRepository<Interior> _interiorRepo;
        public IRepository<Interior> InteriorRepo
        {
            get
            {
                if (_interiorRepo is null)
                    _interiorRepo = new Repository<Interior>(_mongoClient);
                return _interiorRepo;
            }
        }


        private IRepository<Blog> _blogRepo;
        public IRepository<Blog> BlogRepo
        {
            get
            {
                if (_blogRepo is null)
                    _blogRepo = new Repository<Blog>(_mongoClient);
                return _blogRepo;
            }
        }

        private IRepository<BlogComment> _blogCommentRepo;
        public IRepository<BlogComment> BlogCommentRepo
        {
            get
            {
                if (_blogCommentRepo is null)
                    _blogCommentRepo = new Repository<BlogComment>(_mongoClient);
                return _blogCommentRepo;
            }
        }

        private IRepository<Contact> _contactRepo;
        public IRepository<Contact> ContactRepo
        {
            get
            {
                if (_contactRepo is null)
                    _contactRepo = new Repository<Contact>(_mongoClient);
                return _contactRepo;
            }
        }
    }
}
