using electro_shop_backend.Exceptions.CustomExceptions;
using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.Discount;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "AdminPolicy")]
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
        [Authorize(Policy = "AdminPolicy")]
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

        [HttpGet("{discountId}/products")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetDiscountProducts(int discountId)
        {
            var result = await _discountService.GetDiscountedProductsAsync(discountId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateDiscount([FromBody] CreateDiscountRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _discountService.CreateDiscountAsync(requestDto);
                return CreatedAtAction(nameof(GetDiscountById), new { discountId = result.DiscountId }, result.ToDiscountDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định xảy ra trong DiscountController.");
                return StatusCode(500);
            }
        }

        [HttpPost("apply")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ApplyDiscountToProducts([FromBody] ApplyDiscountDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                int productCount = await _discountService.ApplyDiscountToProductsAsync(requestDto);
                return Ok(new 
                { 
                    Message = "Khuyến mãi đã được áp dụng thành công.",
                    ProductCount = productCount
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Có lỗi xảy ra khi áp dụng khuyến mãi.", Error = ex.Message });
            }
        }

        [HttpPut("{discountId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateDiscount([FromRoute] int discountId, [FromBody] CreateDiscountRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _discountService.UpdateDiscountAsync(discountId, requestDto);
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

        [HttpDelete("{discountId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteDiscount([FromRoute] int discountId)
        {
            try
            {
                await _discountService.DeleteDiscountAsync(discountId);
                return NoContent();
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

    }
}
