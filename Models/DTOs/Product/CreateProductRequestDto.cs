using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Product
{
    public class CreateProductRequestDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(500)]
        public required string Info { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }
    }
}
