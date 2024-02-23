using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository.Model;
using Repository.Models;
using Repository.ModelView;
using Repository.Repository;
using Repository.Tools;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Repository.ModelView.BlogView;

namespace Service.Service
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly ILogger<BlogService> _logger;
        public BlogService(IUnitOfWork unit, IRepository<AccountStatus> repoAccountStatus, IMapper mapper, ILogger<BlogService> logger)
        {
            _unit = unit;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> AddBlog(AddBlogView add)
        {
            string _id = Authentication.GetUserIdFromJwt(add.Jwt);
            IEnumerable<AccountStatus> getUser = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["_id", "IsRole"],
                            g => g.AccountId.Equals(_id));
            if (getUser.Any())
            {
                AccountStatus getFieldsFromUser = getUser.First();
                if (getFieldsFromUser.IsRole == AccountStatus.Role.Staff)
                {
                    Blog blog = _mapper.Map<Blog>(add);
                    blog.AccountId = getFieldsFromUser.AccountId;
                    await _unit.BlogRepo.AddOneItem(blog);
                    return "Add Blog successfully";
                }
                return "You have not permission to use this function";
            }
            return "Account is not existed";
        }


        public async Task<string> UpdateBlog(UpdateBlogView update)
        {
            string _id = Authentication.GetUserIdFromJwt(update.Jwt);
            IEnumerable<AccountStatus> getUser = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["_id", "IsRole"],
                            g => g.AccountId.Equals(_id));
            if (getUser.Any())
            {
                AccountStatus getFieldsFromUser = getUser.First();
                if (getFieldsFromUser.IsRole == AccountStatus.Role.Staff)
                {
                    IEnumerable<Blog> getBlog = await _unit.BlogRepo.GetFieldsByFilterAsync([],
                            g => g.BlogId.Equals(update.BlogId));
                    Blog blog = getBlog.FirstOrDefault()!;
                    if (blog is not null && blog.AccountId.Equals(getFieldsFromUser.AccountId))
                    {
                        blog.UpdatedAt = DateTime.Now;
                        await _unit.BlogRepo.UpdateItemByValue("_id", update.BlogId, blog);
                        return "Update Blog successfully";
                    }
                    return "Blog is not existed";
                }
                return "You have not permission to use this function";
            }
            return "Account is not existed";
        }

        public async Task<string> RemoveBlog(RemoveBlogView remove)
        {
            string _id = Authentication.GetUserIdFromJwt(remove.Jwt);
            IEnumerable<AccountStatus> getUser = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["_id", "IsRole"],
                            g => g.AccountId.Equals(_id));
            if (getUser.Any())
            {
                AccountStatus getFieldsFromUser = getUser.First();
                if (getFieldsFromUser.IsRole == AccountStatus.Role.Staff)
                {
                    IEnumerable<Blog> getBlog = await _unit.BlogRepo.GetFieldsByFilterAsync([],
                            g => g.BlogId.Equals(remove.BlogId));
                    Blog blog = getBlog.FirstOrDefault()!;
                    if (blog is not null && blog.AccountId.Equals(getFieldsFromUser.AccountId))
                    {
                        await _unit.BlogRepo.RemoveItemByValue("_id", remove.BlogId);
                        return "Remove Blog successfully";
                    }
                    return "Blog is not existed";
                }
                return "You have not permission to use this function";
            }
            return "Account is not existed";
        }

    }
}
