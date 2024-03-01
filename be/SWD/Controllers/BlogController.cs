using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using Services.Service;
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

        [HttpPost("Add-An-Blog")]
        public async Task<IActionResult> AddAnBlog(AddBlogView add)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var status = await _blogService.AddBlog(add);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("Update-An-Blog")]
        public async Task<IActionResult> UpdateAnBlog(UpdateBlogView update)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var status = await _blogService.UpdateBlog(update);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Remove-An-Blog")]
        public async Task<IActionResult> RemoveAnBlog([FromBody] RemoveBlogView remove)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var status = await _blogService.RemoveBlog(remove);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Get-Paging-Blog-List")]
        public async Task<IActionResult> GetPagingBlogList(int pageIndex, bool isAsc, string? searchValue)
        {
            var result = await _blogService.GetPagingBlog(pageIndex, isAsc, searchValue);
            return Ok(result);
        }

        [HttpPost("Add-An-Comment-Blog")]
        public async Task<IActionResult> AddAnCommentBlog([FromBody] AddCommentBlogView addComment)
        {
            try
            {
                await _blogService.AddBlogComment(addComment);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("Update-An-Comment-Blog")]
        public async Task<IActionResult> UpdateCommentAnBlog([FromBody] UpdateCommentBlogView updateComment)
        {
            try
            {
                await _blogService.UpdateCommentBlog(updateComment);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Remove-An-Comment-Blog")]
        public async Task<IActionResult> RemoveAnCommentBlog([FromBody] RemoveCommentBlogView removeComment)
        {
            try
            {
                await _blogService.RemoveCommentBlog(removeComment);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
