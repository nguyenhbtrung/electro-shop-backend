using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Product
{
    public class UpdateProductRequestDto
    {
        [StringLength(255)]
        public string Name { get; set; } = null!;

        [StringLength(500)]
        public string Info { get; set; } = null!;

        public decimal Price { get; set; }

        public int Stock { get; set; }
        public List<int> CategoryIds { get; set; } = new();
        public int BrandId { get; set; }

    }
}
