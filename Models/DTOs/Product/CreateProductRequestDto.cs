using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Product
{
    public class CreateProductRequestDto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(500)]
        public string Info { get; set; } = null!;

        [Required]
        public decimal Price { get; set; } 

        [Required]
        public int Stock { get; set; }
    }
}
