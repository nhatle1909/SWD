using Repositories.Model;
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
        Task<string> AddBlog(AddBlogView add);
        Task<string> UpdateBlog(UpdateBlogView update);
        Task<string> RemoveBlog(RemoveBlogView remove);
        Task<object> GetPagingBlog(int pageIndex, bool isAsc, string? searchValue);
        Task AddBlogComment(AddCommentBlogView addComment);
        Task UpdateCommentBlog(UpdateCommentBlogView updateComment);
        Task RemoveCommentBlog(RemoveCommentBlogView removeComment);
    }
}
