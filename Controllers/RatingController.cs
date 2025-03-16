using electro_shop_backend.Extensions;
using electro_shop_backend.Models.DTOs.Rating;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        private readonly UserManager<User> _userManager;

        public RatingController(IRatingService ratingService, UserManager<User> userManager)
        {
            _ratingService = ratingService;
            _userManager = userManager;
        }

        [HttpGet] //Lấy tất cả đánh giá
        public async Task<IActionResult> GetAllRating()
        {
            var ratings = await _ratingService.GetAllRatingAsync();
            return Ok(ratings);
        }

        [HttpGet("product/{ProductId}")] //Lấy đánh giá theo ProductId
        public async Task<IActionResult> GetRatingByProductId(int ProductId)
        {
            var rating = await _ratingService.GetRatingByProductIdAsync(ProductId);
            if (rating == null) return NotFound("Không tìm thấy đánh giá");
            return Ok(rating);
        }

        [HttpGet("user/{UserId}")] //Lấy đánh giá theo UserId
        public async Task<IActionResult> GetRatingByUserId(string UserId)
        {
            var rating = await _ratingService.GetRatingByUserIdAsync(UserId);
            if (rating == null) return NotFound("Không tìm thấy đánh giá");
            return Ok(rating);
        }

        [HttpPost] //Tạo đánh giá
        public async Task<IActionResult> CreateRating([FromBody] CreateRatingRequestDto requestDto)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _ratingService.CreateRatingAsync(user.Id, requestDto);
            return Ok(result);
        }

        [HttpPut("{ProductId}")] // Cập nhật đánh giá
        public async Task<IActionResult> UpdateRating(int ProductId, [FromBody] UpdateRatingDto requestDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _ratingService.UpdateRatingAsync(ProductId, requestDto, userId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{ProductId}")] //Xóa đánh giá
        public async Task<IActionResult> DeleteRating(int ProductId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin"); // Kiểm tra xem người dùng có phải admin không
            await _ratingService.DeleteRatingAsync(ProductId, userId, isAdmin);
            return Ok("Rating deleted successfully");
        }
    }

}
