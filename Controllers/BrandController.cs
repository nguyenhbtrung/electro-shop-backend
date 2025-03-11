using electro_shop_backend.Exceptions;
using electro_shop_backend.Models.DTOs.Brand;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController:ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBrand()
        {
            var brand = await _brandService.GetAllBrandAsync();
            return Ok(brand);
        }
        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateBrand([FromBody] CreateBrandRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _brandService.CreateBrandAsync(requestDto);
            return Ok(result);
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] UpdateBrandRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _brandService.UpdateBrandAsync(id, requestDto);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Lỗi khi cập nhật danh mục sản phẩm :{e.Message}");
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            try
            {
                var result = await _brandService.DeleteBrandAsync(id);
                return result ? NoContent() : NotFound("Không tìm thấy Nhãn hàng.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Lỗi khi xóa nhãn hàng: {e.Message}");
            }
        }
        //các api khác CRUD
        [HttpGet("{id}/Product")]
        public async Task<IActionResult> GetAllProdcutByBrandId(int id)
        {
            var brand =await _brandService.GetAllProdcutByBrandIdAsync(id);
            if (brand == null) return NotFound("Không tìm thấy nhãn hàng naò");
            return Ok(brand);
        }
    }
}
