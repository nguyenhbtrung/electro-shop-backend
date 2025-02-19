using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace electro_shop_backend.Models.DTOs.Category
{
    public class UpdateCategoryRequestDto
    {
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(255)]
        public string? Description { get; set; }

        public int? ParentCategoryId { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }
    }
}
