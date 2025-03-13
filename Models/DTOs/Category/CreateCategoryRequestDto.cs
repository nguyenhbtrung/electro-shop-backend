using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace electro_shop_backend.Models.DTOs.Category
{
    public class CreateCategoryRequestDto
    {
        [StringLength(100)]
        [Required]
        public string Name { get; set; } = null!;

        [StringLength(255)]
        [Required]
        public string? Description { get; set; }

        public int? ParentCategoryId { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }
    }
}