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

        public async Task<(bool, string)> AddBlog(string id, AddBlogView add)
        {
            var getUser = (await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["Email"],
                            g => g.AccountId.Equals(id))).FirstOrDefault();
            if (getUser != null)
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
                    blog.Email = getUser.Email;
                    blog.Pictures = picturesBytesList;
                    await _unit.BlogRepo.AddOneItem(blog);
                    return (true, "Add Blog successfully");
                }
                return (false, "Missing the Pictures");
            }
            return (true, "Account is not existed");
        }

        public async Task<(bool, string)> UpdateBlog(string id, UpdateBlogView update)
        {
            var getUser = (await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["Email"],
                            g => g.AccountId.Equals(id))).FirstOrDefault();
            if (getUser != null)
            {
                var getBlog = await _unit.BlogRepo.GetFieldsByFilterAsync([],
                        g => g.BlogId.Equals(update.BlogId));
                Blog blog = getBlog.FirstOrDefault()!;
                if (blog is not null && blog.Email.Equals(getUser.Email))
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
                        blog.UpdatedAt = DateTime.UtcNow;
                        await _unit.BlogRepo.UpdateItemByValue("BlogId", update.BlogId, blog);
                        return (true, "Update Blog successfully");

                    }
                    return (false, "Missing the Pictures");
                }
                return (false, "Blog is not existed");
            }
            return (false, "Account is not existed");
        }

        public async Task<(bool, string)> RemoveBlog(string id, RemoveBlogView remove)
        {
            var getUser = (await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["Email"],
                            g => g.AccountId.Equals(id))).FirstOrDefault();
            if (getUser != null)
            {
                var getBlog = await _unit.BlogRepo.GetFieldsByFilterAsync([],
                    g => g.BlogId.Equals(remove.BlogId));
                Blog blog = getBlog.FirstOrDefault()!;
                if (blog is not null && blog.Email.Equals(getUser.Email))
                {
                    await _unit.BlogRepo.RemoveItemByValue("BlogId", remove.BlogId);
                    return (true, "Remove Blog successfully");
                }
                return (false, "Blog is not existed");
            }
            return (false, "Account is not existed");
        }

        public async Task<object> GetPagingBlog(int pageIndex, bool isAsc, string? searchValue)
        {
            const int pageSize = 5;
            const string sortField = "CreatedAt";
            List<string> searchFields = ["Title", "Content"];
            List<string> returnFields = [];

            int skip = (pageIndex - 1) * pageSize;
            var items = (await _unit.BlogRepo.PagingAsync(skip, pageSize, isAsc, sortField, searchValue, searchFields, returnFields)).ToList();
            return items;
        }
        public async Task<(bool, Blog)> ViewBlogDetail(string _id) 
        {
            IEnumerable<Blog> blog = await _unit.BlogRepo.GetByFilterAsync(b => b.BlogId.Equals(_id));
            if (blog.Any()) 
            {
                Blog blogDetail = _mapper.Map<Blog>(blog);
                blogDetail.BlogId = _id;
                blogDetail.Pictures = blog.FirstOrDefault().Pictures;
                blogDetail.Title = blog.First().Title;
                blogDetail.Content = blog.First().Content;
                blogDetail.CreatedAt = blog.FirstOrDefault().CreatedAt;
                blogDetail.UpdatedAt = blog.FirstOrDefault().UpdatedAt;
                blogDetail.Email = blog.FirstOrDefault().Email;
                return (true, blogDetail);
            }
            return (false, null);
        }

        public async Task AddBlogComment(string id, AddCommentBlogView addComment)
        {
            var getUser = (await _unit.AccountRepo.GetFieldsByFilterAsync(["Email"],
                            g => g.Email.Equals(id))).FirstOrDefault();
            if (getUser != null)
            {
                BlogComment blogComment = _mapper.Map<BlogComment>(addComment);
                blogComment.BlogId = addComment.BlogId;
                blogComment.Email = getUser.Email;
                await _unit.BlogCommentRepo.AddOneItem(blogComment);
            }
        }

        public async Task UpdateCommentBlog(string id, UpdateCommentBlogView updateComment)
        {
            var getUser = (await _unit.AccountRepo.GetFieldsByFilterAsync(["Email"],
                            g => g.Email.Equals(id))).FirstOrDefault();
            var getComment = (await _unit.BlogCommentRepo.GetFieldsByFilterAsync([],
                            g => g.BlogCommentId.Equals(updateComment.BlogCommentId))).FirstOrDefault();
            if (getComment != null && getComment.Email.Equals(getUser.Email))
            {
                if (getComment.Email.Equals(getUser.Email))
                {
                    getComment.Comment = updateComment.Comment;
                    getComment.UpdatedAt = DateTime.UtcNow;
                    await _unit.BlogCommentRepo.UpdateItemByValue("BlogCommentId", getComment.BlogCommentId, getComment);
                }
            }
        }

        public async Task RemoveCommentBlog(string id, RemoveCommentBlogView removeComment)
        {
            var getUser = (await _unit.AccountRepo.GetFieldsByFilterAsync(["Email"],
                            g => g.Email.Equals(id))).FirstOrDefault();
            var getComment = (await _unit.BlogCommentRepo.GetFieldsByFilterAsync([],
                            g => g.BlogCommentId.Equals(removeComment.BlogCommentId))).FirstOrDefault();
            if (getComment != null && getComment.Email.Equals(getUser.Email))
            {
                if (getComment.Email.Equals(getUser.Email))
                {
                    await _unit.BlogCommentRepo.RemoveItemByValue("BlogCommentId", getComment.BlogCommentId);
                }
            }
        }
    }
}
