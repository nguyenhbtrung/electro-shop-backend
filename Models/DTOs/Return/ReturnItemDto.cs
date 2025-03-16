using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Return
{
    public class ReturnItemDto
    {
        [Required]
        public int OrderItemId { get; set; }
        [Required]
        public int ReturnQuantity { get; set; }
    }
}
