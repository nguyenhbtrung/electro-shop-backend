using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterController : ControllerBase
    {
        private readonly IFilterService _filterService;
        public FilterController (IFilterService filterService)
        {
            _filterService = filterService;
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string productName)
        {
            if (string.IsNullOrEmpty(productName))
            {
                return BadRequest("Tham số tìm kiếm không được để trống.");
            }

            var productDtos = await _filterService.FindProductByNameAsync(productName);
            return Ok(productDtos);
        }
        [HttpGet("category")]
        public async Task<IActionResult> FilterProductsInCategory(
            [FromQuery] int categoryId,
            [FromQuery] int? priceFilter,
            [FromQuery] int? brandId,
            [FromQuery] int? ratingFilter)
        {
            var productDtos = await _filterService.FilterProductsInCategoryAsync(categoryId,priceFilter, brandId, ratingFilter);
            return Ok(productDtos);
        }
        [HttpGet("brand")]
        public async Task<IActionResult> FilterProductsInBrand(
            [FromQuery] int brandId,
            [FromQuery] int? priceFilter,
            [FromQuery] int? categoryId,
            [FromQuery] int? ratingFilter)
        {
            var productDtos = await _filterService.FilterProductsInBrandAsync(brandId, priceFilter, categoryId, ratingFilter);
            return Ok(productDtos);
        }
    }
}
