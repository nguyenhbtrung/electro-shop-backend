using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Brand
{
    public class UpdateBrandRequestDto
    {
        [Required]
        [StringLength(250)]
        public string? BrandName { get; set; }
        [Required]
        [StringLength(200)]
        public string? Country { get; set; }
        [Required]
        [StringLength(255)]
        public string? ImageUrl { get; set; }
        [Required]
        [StringLength(1000)]
        public string? Info { get; set; }
    }
}
