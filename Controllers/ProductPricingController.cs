using electro_shop_backend.Models.DTOs.Price;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductPricingController : ControllerBase
    {
        private readonly IProductPricingService _pricingService;

        public ProductPricingController(IProductPricingService pricingService)
        {
            _pricingService = pricingService;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> CalculatePrice([FromBody] PriceCalculationDto dto)
        {
            var result = await _pricingService.CalculatePriceAsync(dto.ProductId, dto.SelectedAttributeDetailIds);
            return Ok(result);
        }
    }
}
