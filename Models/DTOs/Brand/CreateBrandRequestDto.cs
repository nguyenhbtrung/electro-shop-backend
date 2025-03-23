using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace electro_shop_backend.Models.DTOs.Brand
{
    public class CreateBrandRequestDto
    {
        [Required]
        [StringLength(250)]
        public string? BrandName { get; set; }
        [Required]
        [StringLength(200)]
        public string? Country { get; set; }
        [StringLength(1000)]
        public string? ImageUrl { get; set; }
        [Required]
        [StringLength(1000)]
        public string? Info { get; set; } 
    }
}
