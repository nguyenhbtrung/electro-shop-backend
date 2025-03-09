using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Order
{
    public class UpdatePaymentRequestDto
    {
        public int PaymentId { get; set; }

        public int OrderId { get; set; }

        public string? PaymentMethod { get; set; }

        public string? PaymentStatus { get; set; }
    }
}
