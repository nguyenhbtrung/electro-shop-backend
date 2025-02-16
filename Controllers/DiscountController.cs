using electro_shop_backend.Exceptions;
using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.Discount;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        private readonly ILogger<DiscountController> _logger;

        public DiscountController(IDiscountService discountService, ILogger<DiscountController> logger)
        {
            _discountService = discountService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetDiscounts([FromQuery] DiscountQuery discountQuery)
        {
            try
            {
                var result = await _discountService.GetDiscountsAsync(discountQuery);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định xảy ra trong DiscountController.");
                return StatusCode(500);
            }
        }

        [HttpGet("{discountId}")]
        public async Task<IActionResult> GetDiscountById([FromRoute] int discountId)
        {
            try
            {
                var result = await _discountService.GetDiscountByIdAsync(discountId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định xảy ra trong DiscountController.");
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscount([FromBody] CreateDiscountRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _discountService.CreateDiscountAsync(requestDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định xảy ra trong DiscountController.");
                return StatusCode(500);
            }
        }
    }
}
