using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services.Interfaces;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductIdsAndNamesAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound("Không tìm thấy sản phẩm");
            return Ok(product);
        }
    }
}
