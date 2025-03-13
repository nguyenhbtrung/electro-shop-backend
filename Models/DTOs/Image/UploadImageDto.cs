using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Image
{
    public class UploadImageDto
    {
        [Required]
        public required IFormFile File { get; set; }
    }
}
