using electro_shop_backend.Exceptions;
using electro_shop_backend.Extensions;
using electro_shop_backend.Models.DTOs.Cart;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;
        private readonly UserManager<User> _userManager;

        public CartController(ICartService cartService, ILogger<CartController> logger, UserManager<User> userManager)
        {
            _cartService = cartService;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet("admin/allcart")]
        public async Task<IActionResult> GetAllCartASyncs()
        {
            var listCart = await _cartService.GetAllCartASyncs();
            return Ok(listCart);
        }

        [HttpPost("user/addtocart")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> AddToCartAsync(int productId, int quantity)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                var cartItem = await _cartService.AddToCartAsync(user.Id, productId, quantity);
                return Ok(cartItem);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("user/viewcart")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetCartByUserIdAsync()
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            var cart = await _cartService.GetCartByUserIdAsync(user.Id);
            if (cart == null) return NotFound("Không tìm thấy giỏ hàng");
            return Ok(cart);
        }

        [HttpGet("admin/getcartbyuserid/{userId}")]
        public async Task<IActionResult> GetCartByUserIdForAdminAsync(string userId)
        {
            var cart = await _cartService.GetCartByUserIdForAdminAsync(userId);
            if (cart == null) return NotFound("Không tìm thấy giỏ hàng");
            return Ok(cart);
        }

        [HttpGet("admin/getcartbycartid/{cartId}")]
        public async Task<IActionResult> GetCartByCartIdAsync(int cartId)
        {
            var cart = await _cartService.GetCartByCartIdAsync(cartId);
            if (cart == null) return NotFound("Không tìm thấy giỏ hàng");
            return Ok(cart);
        }

        [HttpPut("user/editcartitemquantity")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> UpdateCartItemQuantityAsync(int productId, int quantity)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                var cartItem = await _cartService.UpdateCartItemQuantityAsync(user.Id, productId, quantity);
                return Ok(cartItem);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("admin/editcartitem/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItemQuantityAsync(string userId, int productId, int quantity)
        {
            try
            {
                var cartItem = await _cartService.UpdateCartItemQuantityAsync(userId, productId, quantity);
                return Ok(cartItem);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("user/deletecart")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> DeleteCartAsync()
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                await _cartService.DeleteCartAsync(user.Id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("user/deletecartitem")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> DeleteCartItemAsync(int productId)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                await _cartService.DeleteCartItemAsync(user.Id, productId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("admin/deletecart")]
        public async Task<IActionResult> DeleteCartAdminAsync(string userId)
        {
            try
            {
                await _cartService.DeleteCartAsync(userId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("admin/deletecartitem/{cartItemId}")]
        public async Task<IActionResult> DeleteCartItemAdminAsync(string userId, int cartItemId)
        {
            try
            {
                await _cartService.DeleteCartItemAsync(userId, cartItemId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
