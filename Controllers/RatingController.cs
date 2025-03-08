using electro_shop_backend.Extensions;
using electro_shop_backend.Models.DTOs.Rating;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> GetAllRating()
        {
            var ratings = await _ratingService.GetAllRatingAsync();
            return Ok(ratings);
        }
        [HttpGet("{ProductId}")]
        public async Task<IActionResult> GetRating(int ProductId)
        {
            var rating = await _ratingService.GetRatingAsync(ProductId);
            if (rating == null) return NotFound("Không tìm thấy đánh giá");
            return Ok(rating);
        }
        [HttpPost]
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
        [HttpPut("{ProductId}")]
        public async Task<IActionResult> UpdateRating(int ProductId, [FromBody] UpdateRatingDto requestDto)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _ratingService.UpdateRatingAsync(ProductId, requestDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{ProductId}")]
        public async Task<IActionResult> DeleteRating(int ProductId)
        {
            var result = await _ratingService.DeleteRatingAsync(ProductId);
            return Ok(result);
        }
    }

}
