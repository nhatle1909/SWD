using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using Services.Service;
using System.Security.Claims;
using static Repositories.ModelView.AccountView;
using static Repositories.ModelView.BlogView;

namespace SWD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("Add-An-Blog")]
        public async Task<IActionResult> AddAnBlog(AddBlogView add)
        {
            try
            {
                var id = (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? "";
                var status = await _blogService.AddBlog(id, add);
                return Ok(new
                {
                    Message = status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpPatch("Update-An-Blog")]
        public async Task<IActionResult> UpdateAnBlog(UpdateBlogView update)
        {
            try
            {
                var id = (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? "";
                var status = await _blogService.UpdateBlog(id, update);
                return Ok(new
                {
                    Message = status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpDelete("Remove-An-Blog")]
        public async Task<IActionResult> RemoveAnBlog([FromBody] RemoveBlogView remove)
        {
            try
            {
                var id = (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? "";
                var status = await _blogService.RemoveBlog(id, remove);
                return Ok(new
                {
                    Message = status
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("Get-Paging-Blog-List")]
        public async Task<IActionResult> GetPagingBlogList(int pageIndex, bool isAsc, string? searchValue)
        {
            var result = await _blogService.GetPagingBlog(pageIndex, isAsc, searchValue);
            return Ok(new
            {
                Message = result
            }); ;
        }

        [Authorize]
        [HttpPost("Add-An-Comment-Blog")]
        public async Task<IActionResult> AddAnCommentBlog([FromBody] AddCommentBlogView addComment)
        {
            try
            {
                var id = (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? "";
                await _blogService.AddBlogComment(id, addComment);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPatch("Update-An-Comment-Blog")]
        public async Task<IActionResult> UpdateCommentAnBlog([FromBody] UpdateCommentBlogView updateComment)
        {
            try
            {
                var id = (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? "";
                await _blogService.UpdateCommentBlog(id, updateComment);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("Remove-An-Comment-Blog")]
        public async Task<IActionResult> RemoveAnCommentBlog([FromBody] RemoveCommentBlogView removeComment)
        {
            try
            {
                var id = (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? "";
                await _blogService.RemoveCommentBlog(id, removeComment);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
