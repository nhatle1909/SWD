using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using Services.Service;
using static Repositories.ModelView.CartView;

namespace SWD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        //[Authorize(Roles = "Customer")]
        //[HttpPost("Customer/Add-An-Product-To-Cart")]
        //public async Task<IActionResult> AddProductToCart(AddCartView add)
        //{
        //    try
        //    {
        //        var id = (HttpContext.User.FindFirst("id")?.Value) ?? "";
        //        var status = await _cartService.AddProductFromCart(id, add);
        //        return Ok(status);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[Authorize(Roles = "Customer")]
        //[HttpPost("Customer/Delete-An-Product-To-Cart")]
        //public async Task<IActionResult> DeleteProductToCart(string interiorId)
        //{
        //    try
        //    {
        //        var status = await _cartService.DeleteProductFromCart(interiorId);
        //        return Ok(status);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[Authorize(Roles = "Customer")]
        //[HttpPost("Customer/Get-All-Product-To-Cart")]
        //public async Task<IActionResult> GetAllProductToCart()
        //{
        //    try
        //    {
        //        var id = (HttpContext.User.FindFirst("id")?.Value) ?? "";
        //        var status = await _cartService.GetAllProductToCart(id);
        //        return Ok(status);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
