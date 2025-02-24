using electro_shop_backend.Exceptions;
using electro_shop_backend.Extensions;
using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.ProductViewHistory;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductViewHistoryController : ControllerBase
    {
        private readonly IProductViewHistoryService _productViewHistoryService;
        private readonly ILogger<ProductViewHistoryController> _logger;
        private readonly UserManager<User> _userManager;

        public ProductViewHistoryController(IProductViewHistoryService productViewHistoryService, ILogger<ProductViewHistoryController> logger, UserManager<User> userManager)
        {
            _productViewHistoryService = productViewHistoryService;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetProductViewHistories([FromQuery] HistoryQuery historyQuery)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                var result = await _productViewHistoryService.GetProductViewHistoriesAsync(user.Id, historyQuery);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định xảy ra trong DiscountController.");
                return StatusCode(500);
            }
        }

        [HttpGet("{historyId}")]
        public async Task<IActionResult> GetProductViewHistoryById([FromRoute] int historyId)
        {
            return Ok("Get");
        }

        [HttpPost("{productId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> CreateProductViewHistory([FromRoute] int productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                var result = await _productViewHistoryService.CreateProductViewHistoryAsync(user.Id, productId);
                return CreatedAtAction(nameof(GetProductViewHistoryById), new { historyId = result.HistoryId }, result.ToProductViewHistoryDto());
            }
            catch (ConflictException)
            {
                return Conflict("Bản ghi đã tồn tại");
            }
            catch (BadRequestException)
            {
                return BadRequest("Không tìm thấy sản phẩm");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định xảy ra trong ProductViewHistoryController.");
                return StatusCode(500);
            }
        }

        [HttpPut("{historyId}")]
        public async Task<IActionResult> UpdateProductViewHistory([FromRoute] int historyId)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                var result = await _productViewHistoryService.UpdateProductViewHistoryAsync(user.Id, historyId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "Khong tim thay ban ghi.");
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex, "Khong tim thay san pham.");
                return BadRequest("Khong tim thay san pham");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định xảy ra trong ProductViewHistoryController.");
                return StatusCode(500);
            }
        }

        [HttpDelete("{historyId}")]
        public async Task<IActionResult> DeleteProductViewHistory([FromRoute] int historyId)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                await _productViewHistoryService.DeleteProductViewHistoryAsync(user.Id, historyId);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "Khong tim thay ban ghi.");
                return NoContent();
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex, "Khong tim thay san pham.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định xảy ra trong ProductViewHistoryController.");
                return NoContent();
            }
        }

        [HttpDelete]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> DeleteAllProductViewHistories()
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                await _productViewHistoryService.DeleteAllProductViewHistoriesAsync(user.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định xảy ra trong ProductViewHistoryController.");
                return StatusCode(500);
            }
        }
    }
}
