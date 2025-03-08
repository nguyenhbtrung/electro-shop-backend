using electro_shop_backend.Models.DTOs.Rating;
using electro_shop_backend.Services;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _ratingService.CreateRatingAsync(requestDto);
            return Ok(result);
        }
        [HttpPut("{ProductId}")]
        public async Task<IActionResult> UpdateRating(int ProductId, [FromBody] UpdateRatingDto requestDto)
        {
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
