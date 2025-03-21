using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Order
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }

        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public string? PaymentMethod { get; set; } // cod, vnpay

        public string? PaymentStatus { get; set; }

        public DateTime? TransactionTime { get; set; }
    }
}