using electro_shop_backend.Exceptions.CustomExceptions;
using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.Discount;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers.V1
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
            var result = await _discountService.GetDiscountsAsync(discountQuery);
            return Ok(result);
        }

        [HttpGet("{discountId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetDiscountById([FromRoute] int discountId)
        {
            var result = await _discountService.GetDiscountByIdAsync(discountId);
            return Ok(result);
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
            var result = await _discountService.CreateDiscountAsync(requestDto);
            return CreatedAtAction(nameof(GetDiscountById), new { discountId = result.DiscountId }, result.ToDiscountDto());
        }

        [HttpPost("apply")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ApplyDiscountToProducts([FromBody] ApplyDiscountDto requestDto)
        {
            int productCount = await _discountService.ApplyDiscountToProductsAsync(requestDto);
            return Ok(new
            {
                Message = "Khuyến mãi đã được áp dụng thành công.",
                ProductCount = productCount
            });

        }

        [HttpPut("{discountId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateDiscount([FromRoute] int discountId, [FromBody] CreateDiscountRequestDto requestDto)
        {
            var result = await _discountService.UpdateDiscountAsync(discountId, requestDto);
            return Ok(result);
        }

        [HttpDelete("{discountId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteDiscount([FromRoute] int discountId)
        {
            await _discountService.DeleteDiscountAsync(discountId);
            return NoContent();
        }

    }
}
