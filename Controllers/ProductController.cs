using Microsoft.AspNetCore.Mvc;
using electro_shop_backend.Services.Interfaces;
using electro_shop_backend.Models.DTOs.Product;
using electro_shop_backend.Models.DTOs.ProductImage;
using electro_shop_backend.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductImageService _productimageService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsIdsAndNamesAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound("Không tìm thấy sản phẩm");
            return Ok(product);
        }


        [HttpPost]
        [Authorize(Policy ="AdminPolicy")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.CreateProductAsync(requestDto);
            return Ok(result);
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _productService.UpdateProductAsync(id, requestDto);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Lỗi khi cập nhật sản phẩm.");
            }
        }
        [HttpPost("{id}/Image")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateProductImage([FromBody] CreateProductImageDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productimageService.CreateProductImageAsync(requestDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);
                return result ? NoContent() : NotFound("Không tìm thấy sản phẩm.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Lỗi khi xóa sản phẩm.");
            }
        }
        [HttpDelete("{id}/Image")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteProductImage(int id)
        {
            try
            {
                var result = await _productimageService.DeleteProductImageAsync(id);
                return result ? NoContent() : NotFound("Không tìm thấy sản phẩm.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Lỗi khi xóa sản phẩm.");
            }
        }
    }
}
