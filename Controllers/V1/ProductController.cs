﻿using Microsoft.AspNetCore.Mvc;
using electro_shop_backend.Services.Interfaces;
using electro_shop_backend.Models.DTOs.Product;
using electro_shop_backend.Models.DTOs.ProductImage;
using Microsoft.AspNetCore.Authorization;
using electro_shop_backend.Helpers;
using Microsoft.EntityFrameworkCore;
using electro_shop_backend.Services;
using electro_shop_backend.Exceptions.CustomExceptions;
using electro_shop_backend.Extensions;
using Microsoft.AspNetCore.Identity;
using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductImageService _productimageService;
        private readonly IProductAttributeService _productAttributeService;
        private readonly UserManager<User> _userManager;

        public ProductController(IProductService productService, IProductImageService productimageService, IProductAttributeService productAttributeService, UserManager<User> userManager)
        {
            _productService = productService;
            _productimageService = productimageService;
            _productAttributeService = productAttributeService;
            _userManager = userManager;
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
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username ?? "");
            var product = await _productService.GetProductByIdAsync(id, user?.Id);
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
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username ?? "");
            var products = await _productService.GetAllProductsByUserAsync(productQuery, user?.Id);
            return Ok(products);
        }

        [HttpGet("discounted")]
        public async Task<IActionResult> GetDiscountedProducts([FromQuery] ProductQuery productQuery)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username ?? "");
            var products = await _productService.GetDiscountedProductsAsync(productQuery, user?.Id);
            return Ok(products);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
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

        [HttpPost("{productId}/Image")]
        [Authorize]
        public async Task<IActionResult> UploadProductImages(int productId, [FromForm] CreateProductImageDto requestDto)
        {
            try
            {
                var result = await _productimageService.CreateProductImagesAsync(productId, requestDto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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

        [HttpGet("recommend/{productId}")]
        public async Task<IActionResult> GetRecommendedProducts(int productId)
        {
            try
            {
                var recommendedProducts = await _productService.GetRecommendedProductsAsync(productId);
                return Ok(recommendedProducts);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
