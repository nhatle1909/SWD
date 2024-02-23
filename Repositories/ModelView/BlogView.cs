using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelView
{
    public class BlogView
    {
        public class AddBlogView
        {
            [Required] public required string Jwt { get; set; }
            [StringLength(100, MinimumLength = 1)] public required string Title { get; set; }
            [StringLength(10000, MinimumLength = 1)] public required string Content { get; set; }
        }

        public class UpdateBlogView
        {
            [Required] public required string BlogId { get; set; }
            [Required] public required string Jwt { get; set; }
            [StringLength(100, MinimumLength = 1)] public required string Title { get; set; }
            [StringLength(10000, MinimumLength = 1)] public required string Content { get; set; }
        }

        public class RemoveBlogView
        {
            [Required] public required string BlogId { get; set; }
            [Required] public required string Jwt { get; set; }
        }
    }
}
