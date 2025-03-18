using Microsoft.AspNetCore.Mvc;
using electro_shop_backend.Services.Interfaces;
using electro_shop_backend.Models.DTOs.Product;
using electro_shop_backend.Models.DTOs.ProductImage;
using electro_shop_backend.Exceptions;
using Microsoft.AspNetCore.Authorization;
using electro_shop_backend.Helpers;
using Microsoft.EntityFrameworkCore;
using electro_shop_backend.Services;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductImageService _productimageService;
        private readonly IProductAttributeService _productAttributeService;
        public ProductController(IProductService productService, IProductImageService productImageService, IProductAttributeService productAttributeService)
        {
            _productService = productService;
            _productimageService = productImageService;
            _productAttributeService = productAttributeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound("Không tìm thấy sản phẩm");
            return Ok(product);
        }

        [HttpGet("by_discount")]
        public async Task<IActionResult> GetProductsByDiscountId([FromQuery] int? discount_id, [FromQuery] string? search)
        {
            var products = await _productService.GetProductsByDiscountIdAsync(discount_id, search);
            return Ok(products);
        }

        [HttpGet("by_user")]
        public async Task<IActionResult> GetAllProductsByUser([FromQuery] ProductQuery productQuery)
        {
            var products = await _productService.GetAllProductsByUserAsync(productQuery);
            return Ok(products);
        }

        [HttpGet("discounted")]
        public async Task<IActionResult> GetDiscountedProducts([FromQuery] ProductQuery productQuery)
        {
            var products = await _productService.GetDiscountedProductsAsync(productQuery);
            return Ok(products);
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
        public async Task<IActionResult> CreateProductImage(int id ,[FromBody] CreateProductImageDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productimageService.CreateProductImageAsync(id,requestDto);
            return Ok(result);
        }
        [HttpPut("{id}/Image")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateProductImage(int id, [FromBody] CreateProductImageDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productimageService.UpdateProductImageAsync(id, requestDto);
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
            catch (Exception e)
            {
                return new ObjectResult(e);
            }
        }
        [HttpDelete("Image/{imageId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteProductImage(int imageId)
        {
            try
            {
                var result = await _productimageService.DeleteProductImageAsync(imageId);
                return result ? NoContent() : NotFound("Không tìm thấy ảnh sản phẩm.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Lỗi khi xóa ảnh sản phẩm.");
            }
        }
        [HttpPost("{productId}/addAttributeId")]
        public async Task<IActionResult> AddAttributeDetails(int productId, [FromBody] AddAttributeDto dto)
        {
            return await _productAttributeService.AssignAttributeDetails(productId, dto.AttributeDetailId);
        }
    }
}
