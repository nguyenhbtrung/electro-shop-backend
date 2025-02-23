using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.DTOs.Voucher
{
    public class AllVoucherDto
    {
        public int VoucherId { get; set; }

        public string VoucherCode { get; set; }  // Mã voucher

        public string? VoucherName { get; set; }  // Tên voucher

        public decimal DiscountValue { get; set; }  // Giá trị giảm giá

        public string VoucherStatus { get; set; } = "active";  // Trạng thái voucher (active/disable)
    }
}
