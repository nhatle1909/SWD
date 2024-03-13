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
        [HttpPost("Staff/Add-An-Blog")]
        public async Task<IActionResult> AddAnBlog(AddBlogView add)
        {
            try
            {
                var id = (HttpContext.User.FindFirst("id")?.Value) ?? "";
                var status = await _blogService.AddBlog(id, add);
                if (status.Item1)
                    return Ok(status.Item2);
                else return BadRequest(status.Item2);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpPatch("Staff/Update-An-Blog")]
        public async Task<IActionResult> UpdateAnBlog(UpdateBlogView update)
        {
            try
            {
                var id = (HttpContext.User.FindFirst("id")?.Value) ?? "";
                var status = await _blogService.UpdateBlog(id, update);
                if (status.Item1)
                    return Ok(status.Item2);
                else return BadRequest(status.Item2);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpDelete("Staff/Remove-An-Blog")]
        public async Task<IActionResult> RemoveAnBlog([FromBody] RemoveBlogView remove)
        {
            try
            {
                var id = (HttpContext.User.FindFirst("id")?.Value) ?? "";
                var status = await _blogService.RemoveBlog(id, remove);
                if (status.Item1)
                    return Ok(status.Item2);
                else return BadRequest(status.Item2);
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
            return Ok(result); ;
        }

        [Authorize]
        [HttpPost("Authorize/Add-An-Comment-Blog")]
        public async Task<IActionResult> AddAnCommentBlog([FromBody] AddCommentBlogView addComment)
        {
            try
            {
                var id = (HttpContext.User.FindFirst("id")?.Value) ?? "";
                await _blogService.AddBlogComment(id, addComment);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPatch("Authorize/Update-An-Comment-Blog")]
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
        [HttpDelete("Authorize/Remove-An-Comment-Blog")]
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
        [HttpGet("Get-Blog-Detail")]
        public async Task<IActionResult> ViewBlogDetail(string _id) 
        {
            var status = await _blogService.ViewBlogDetail(_id);
            if (status.Item1 == false) return BadRequest("Blog does not exist");
            else return Ok(status.Item2);
        }
    }
}
