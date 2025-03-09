using electro_shop_backend.Exceptions;
using electro_shop_backend.Models.DTOs.Category;
using electro_shop_backend.Services;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController: ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
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
        [HttpGet("{id}/Product")]
        public async Task<IActionResult> GetAllProdcutByCategoryId(int id)
        {
            var category = await _categoryService.GetAllProductsByCategoryIdAsync(id);
            if (category == null) return NotFound("Không tìm thấy danh mục sản phẩm");
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
