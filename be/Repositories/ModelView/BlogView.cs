using Microsoft.AspNetCore.Http;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ModelView
{
    public class BlogView
    {
        public class AddBlogView
        {
            [StringLength(100, MinimumLength = 1)] public required string Title { get; set; }
            [StringLength(10000, MinimumLength = 1)] public required string Content { get; set; }
            public required IFormFile[] Pictures { get; set; }
        }

        public class UpdateBlogView
        {
            public required string BlogId { get; set; }
            [StringLength(100, MinimumLength = 1)] public required string Title { get; set; }
            [StringLength(10000, MinimumLength = 1)] public required string Content { get; set; }
            public required IFormFile[] Pictures { get; set; }
        }

        public class RemoveBlogView
        {
            public required string BlogId { get; set; }
        }

        public class AddCommentBlogView
        {
            public required string BlogId { get; set; }
            [StringLength(10000, MinimumLength = 1)]
            public required string Comment { get; set; }
        }

        public class UpdateCommentBlogView
        {
            public required string BlogCommentId { get; set; }
            [StringLength(10000, MinimumLength = 1)]
            public required string Comment { get; set; }
        }

        public class RemoveCommentBlogView
        {
            public required string BlogCommentId { get; set; }
        }


    }
}
