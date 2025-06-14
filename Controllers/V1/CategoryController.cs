﻿using electro_shop_backend.Exceptions.CustomExceptions;
using electro_shop_backend.Extensions;
using electro_shop_backend.Models.DTOs.Category;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly UserManager<User> _userManager;

        public CategoryController(ICategoryService categoryService, UserManager<User> userManager)
        {
            _categoryService = categoryService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var category = await _categoryService.GetAllCategoriesIdsAndNamesAsync();
            return Ok(category);
        }
        [HttpGet("tree")]
        public async Task<IActionResult> GetAllCategoriesTree()
        {
            var category = await _categoryService.GetCategoryTreeAsync();
            return Ok(category);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound("Không tìm thấy danh mục sản phẩm");
            return Ok(category);
        }
        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _categoryService.CreateCategoryAsync(requestDto);
            return Ok(result);
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _categoryService.UpdateCategoryAsync(id, requestDto);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Lỗi khi cập nhật danh mục sản phẩm.");
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var result = await _categoryService.DeleteCategoryAsync(id);
                return result ? NoContent() : NotFound("Không tìm thấy danh mục sản phẩm.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Lỗi khi xóa danh mục sản phẩm: {e.Message}");
            }
        }
        // các api khác Crud

        [HttpGet("{id}/Product")]
        public async Task<IActionResult> GetAllProdcutByCategoryId(int id)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username ?? "");
            var category = await _categoryService.GetAllProductsByCategoryIdAsync(id, user?.Id);
            if (category == null) return NotFound("Không tìm thấy danh mục sản phẩm");
            return Ok(category);
        }
    }
}
