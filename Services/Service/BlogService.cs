using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Org.BouncyCastle.Crypto;
using Repositories.Model;
using Repositories.Models;
using Repositories.ModelView;
using Repositories.Repository;
using Services.Tool;
using Services.Interface;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Repositories.ModelView.AccountView;
using static Repositories.ModelView.BlogView;

namespace Services.Service
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
            string _id = AuthenticationJwtTool.GetUserIdFromJwt(add.Jwt);
            IEnumerable<AccountStatus> getUser = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["Email", "IsRole"],
                            g => g.AccountId.Equals(_id));
            if (getUser.Any())
            {
                AccountStatus getFieldsFromUser = getUser.First();
                if (getFieldsFromUser.IsRole == AccountStatus.Role.Staff)
                {
                    if (add.Pictures.Length > 0)
                    {
                        List<byte[]> picturesBytesList = new List<byte[]>();
                        foreach (var picture in add.Pictures)
                        {
                            //Encode picture
                            using (var ms = new MemoryStream())
                            {
                                await picture.CopyToAsync(ms);
                                byte[] fileBytes = ms.ToArray();
                                picturesBytesList.Add(fileBytes);
                            }
                        }
                        Blog blog = _mapper.Map<Blog>(add);
                        blog.Email = getFieldsFromUser.Email;
                        blog.Pictures = picturesBytesList;
                        await _unit.BlogRepo.AddOneItem(blog);
                        return "Add Blog successfully";
                    }
                    return "Missing the Pictures";
                }
                return "You have not permission to use this function";
            }
            return "Account is not existed";
        }


        public async Task<string> UpdateBlog(UpdateBlogView update)
        {
            string _id = AuthenticationJwtTool.GetUserIdFromJwt(update.Jwt);
            IEnumerable<AccountStatus> getUser = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["Email", "IsRole"],
                            g => g.AccountId.Equals(_id));
            if (getUser.Any())
            {
                AccountStatus getFieldsFromUser = getUser.First();
                if (getFieldsFromUser.IsRole == AccountStatus.Role.Staff)
                {
                    IEnumerable<Blog> getBlog = await _unit.BlogRepo.GetFieldsByFilterAsync([],
                            g => g.BlogId.Equals(update.BlogId));
                    Blog blog = getBlog.FirstOrDefault()!;
                    if (blog is not null && blog.Email.Equals(getFieldsFromUser.Email))
                    {
                        if (update.Pictures.Length > 0)
                        {
                            List<byte[]> picturesBytesList = new List<byte[]>();
                            foreach (var picture in update.Pictures)
                            {
                                //Encode picture
                                using (var ms = new MemoryStream())
                                {
                                    await picture.CopyToAsync(ms);
                                    byte[] fileBytes = ms.ToArray();
                                    picturesBytesList.Add(fileBytes);
                                }
                            }
                            blog.Title = update.Title;
                            blog.Content = update.Content;
                            blog.Pictures = picturesBytesList;
                            blog.UpdatedAt = DateTime.Now;
                            await _unit.BlogRepo.UpdateItemByValue("BlogId", update.BlogId, blog);
                            return "Update Blog successfully";

                        }
                        return "Missing the Pictures";
                    }
                    return "Blog is not existed";
                }
                return "You have not permission to use this function";
            }
            return "Account is not existed";
        }

        public async Task<string> RemoveBlog(RemoveBlogView remove)
        {
            string _id = AuthenticationJwtTool.GetUserIdFromJwt(remove.Jwt);
            IEnumerable<AccountStatus> getUser = await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["Email", "IsRole"],
                            g => g.AccountId.Equals(_id));
            if (getUser.Any())
            {
                AccountStatus getFieldsFromUser = getUser.First();
                if (getFieldsFromUser.IsRole == AccountStatus.Role.Staff)
                {
                    IEnumerable<Blog> getBlog = await _unit.BlogRepo.GetFieldsByFilterAsync([],
                            g => g.BlogId.Equals(remove.BlogId));
                    Blog blog = getBlog.FirstOrDefault()!;
                    if (blog is not null && blog.Email.Equals(getFieldsFromUser.Email))
                    {
                        await _unit.BlogRepo.RemoveItemByValue("BlogId", remove.BlogId);
                        return "Remove Blog successfully";
                    }
                    return "Blog is not existed";
                }
                return "You have not permission to use this function";
            }
            return "Account is not existed";
        }

        public async Task<object> GetPagingBlog(int pageIndex, bool isAsc, string? searchValue)
        {
            const int pageSize = 5;
            const string sortField = "CreatedAt";
            List<string> searchFields = ["Title", "Content"];
            List<string> returnFields = [];
            List<byte[]> pictures = new List<byte[]>();

            int skip = (pageIndex - 1) * pageSize;
            var items = (await _unit.BlogRepo.PagingAsync(skip, pageSize, isAsc, sortField, searchValue, searchFields, returnFields)).ToList();
            foreach (var item in items)
            {
                var getUser = (await _unit.AccountRepo.GetFieldsByFilterAsync(["Picture"],
                            g => g.Email.Equals(item.Email))).FirstOrDefault();
                pictures.Add(getUser!.Picture);
            }
            var response = new
            {
                Pictures = pictures,
                Items = items
            };
            return response;
        }
    }
}
