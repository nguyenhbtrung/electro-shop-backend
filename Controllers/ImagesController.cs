using electro_shop_backend.Models.DTOs.Image;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

        private readonly IImageService _imageService;
        private readonly ILogger<ImagesController> _logger;

        public ImagesController(IImageService imageService, ILogger<ImagesController> logger)
        {
            _imageService = imageService;
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageDto uploadImageDto)
        {
            if (uploadImageDto.File == null || uploadImageDto.File.Length <= 0)
            {
                return BadRequest("Không có file ảnh được gửi lên.");
            }
            try
            {
                var imageUrl = await _imageService.UploadImageAsync(uploadImageDto.File);
                return Ok(new { ImageUrl = imageUrl });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi xử lý yêu cầu upload ảnh.");
                return StatusCode(500, "Có lỗi xảy ra khi upload ảnh");
            }

        }

        [HttpDelete("delete-by-url")]
        public async Task<IActionResult> DeleteImageByUrl(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return BadRequest("URL ảnh không hợp lệ.");
            }
            var result = await _imageService.DeleteImageByUrlAsync(imageUrl);
            if (result)
                return Ok("Xoá ảnh thành công.");
            else
                return NotFound("Ảnh không tồn tại hoặc đã bị xoá.");
        }
    }
}
