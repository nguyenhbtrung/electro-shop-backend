using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Order
{
    public class AllOrderDto
    {
        public int OrderId { get; set; }

        public string? UserId { get; set; }

        public decimal Total { get; set; }

        public string? Status { get; set; }

        public DateTime? TimeStamp { get; set; }

        public virtual List<UpdatePaymentRequestDto> Payments { get; set; } = new List<UpdatePaymentRequestDto>();
    }
}
