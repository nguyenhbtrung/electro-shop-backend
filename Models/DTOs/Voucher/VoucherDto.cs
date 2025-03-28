using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Voucher
{
    public class VoucherDto
    {
        public int VoucherId { get; set; }

        public string VoucherCode { get; set; }  // Mã voucher

        public string? VoucherName { get; set; }  // Tên voucher

        public string VoucherType { get; set; }  // Kiểu giảm giá ('percentage' hoặc 'fixed')

        public decimal DiscountValue { get; set; }  // Giá trị giảm giá

        public decimal MinOrderValue { get; set; } = 0;  // Giá trị đơn hàng tối thiểu

        public decimal? MaxDiscount { get; set; }  // Giới hạn giảm giá tối đa

        public int? UsageLimit { get; set; }  // Tổng số lần sử dụng

        public int? UsageCount { get; set; } = 0;  // Số lần voucher đã được sử dụng

        public string VoucherStatus { get; set; } = "active";  // Trạng thái voucher (active/disable)

        public DateTime StartDate { get; set; } // Ngày bắt đầu hiệu lực

        public DateTime EndDate { get; set; }  // Ngày hết hạn

        public DateTime CreatedDate { get; set; }  // Ngày tạo
    }
}
