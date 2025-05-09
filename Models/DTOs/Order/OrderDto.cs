using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using electro_shop_backend.Models.DTOs.ProductImage;

namespace electro_shop_backend.Models.DTOs.Order
{
    public class OrderDto
    {
        public int OrderId { get; set; }

        public string? UserId { get; set; }

        public string? FullName { get; set; }

        public decimal Total { get; set; }

        public string? Status { get; set; }

        public string? PaymentStatus { get; set; }

        public string? Address { get; set; }

        public DateTime? TimeStamp { get; set; }

        public string? PaymentMethod { get; set; }

        public string? PaymentUrl { get; set; }

        public List<ProductImageDto> ProductImage { get; set; } = new List<ProductImageDto>();

        public virtual List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

        public virtual List<UpdatePaymentRequestDto> Payments { get; set; } = new List<UpdatePaymentRequestDto>();

        //public virtual List<ReturnDto> Returns { get; set; } = new List<ReturnDto>();
    }
}
