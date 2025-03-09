using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Order
{
    public class UpdateOrderRequestDto
    {
        public int OrderId { get; set; }

        public string? UserId { get; set; }

        public decimal Total { get; set; }

        public string? Status { get; set; }

        public string? Address { get; set; }
    }
}
