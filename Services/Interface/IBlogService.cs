using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Repository.ModelView.AccountView;
using static Repository.ModelView.BlogView;

namespace Service.Interface
{
    public interface IBlogService
    {
        Task<string> AddBlog(AddBlogView add);
        Task<string> UpdateBlog(UpdateBlogView update);
        Task<string> RemoveBlog(RemoveBlogView remove);
    }
}
