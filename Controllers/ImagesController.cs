using electro_shop_backend.Models.DTOs.Image;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

        private readonly IImageService _imageService;
        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageDto uploadImageDto)
        {
            if (uploadImageDto.File == null || uploadImageDto.File.Length <= 0)
            {
                return BadRequest("Không có file ảnh được gửi lên.");
            }
            var imageUrl = await _imageService.UploadImageAsync(uploadImageDto.File);

            return Ok(new { ImageUrl = imageUrl });
        }

    }
}
