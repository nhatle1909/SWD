﻿using Repositories.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Repositories.ModelView.AccountView;
using static Repositories.ModelView.BlogView;

namespace Services.Interface
{
    public interface IBlogService
    {
        Task<string> AddBlog(string id, AddBlogView add);
        Task<string> UpdateBlog(string id, UpdateBlogView update);
        Task<string> RemoveBlog(string id, RemoveBlogView remove);
        Task<object> GetPagingBlog(int pageIndex, bool isAsc, string? searchValue);
        Task AddBlogComment(string id, AddCommentBlogView addComment);
        Task UpdateCommentBlog(string id, UpdateCommentBlogView updateComment);
        Task RemoveCommentBlog(string id, RemoveCommentBlogView removeComment);
    }
}
