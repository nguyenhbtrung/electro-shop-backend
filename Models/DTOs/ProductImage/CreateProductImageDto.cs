using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.ProductImage
{
    public class CreateProductImageDto
    {
        [Required]
        [StringLength(255)]
        public string? ImageUrl { get; set; }
    }
}
