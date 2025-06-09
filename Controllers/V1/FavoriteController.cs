using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace electro_shop_backend.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;
        private readonly UserManager<User> _userManager;

        public FavoriteController(IFavoriteService favoriteService, UserManager<User> userManager)
        {
            _favoriteService = favoriteService;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetAllFavorite()
        {
            var userName = User.Identity?.Name;
            var user = await _userManager.FindByNameAsync(userName); // Lấy user từ UserManager
            var favorites = await _favoriteService.GetAllFavoritesProduct(user.Id);
            return Ok(favorites);
        }
        [HttpPost("toggle/{productId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> ToggleFavorite(int productId)
        {

            var userName = User.Identity?.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var isFavorite = await _favoriteService.ToggleFavoriteAsync(user.Id, productId);
            return Ok(new { status = isFavorite ? "Added" : "Removed" });
        }
    }
}
