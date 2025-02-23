using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Voucher
{
    public class CreateVoucherRequestDto
    {
        [Required]
        public string VoucherCode { get; set; }  // Mã voucher

        public string? VoucherName { get; set; }  // Tên voucher

        [Required]
        public string VoucherType { get; set; }  // Kiểu giảm giá ('percentage' hoặc 'fixed')

        [Required]
        public decimal DiscountValue { get; set; }  // Giá trị giảm giá

        public decimal MinOrderValue { get; set; } = 0;  // Giá trị đơn hàng tối thiểu

        public decimal? MaxDiscount { get; set; }  // Giới hạn giảm giá tối đa

        public int? UsageLimit { get; set; }  // Tổng số lần sử dụng

        [Required]
        public string VoucherStatus { get; set; } = "active"; // Trạng thái voucher (active/disable)

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
