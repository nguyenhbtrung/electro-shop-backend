using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Order
{
    public class OrderItemDto
    {
        public int OrderItemId { get; set; }

        public int? ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string? ProductName { get; set; }

        public string? ProductImage { get; set; }
    }
}
