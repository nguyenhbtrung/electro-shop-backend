using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Models.Entities;

[Table("Voucher")]
[Index("VoucherCode", Name = "UQ__Voucher__217310691457CDB9", IsUnique = true)]
public partial class Voucher
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("voucher_id")] 
    public int VoucherId { get; set; }

    [Required]
    [StringLength(50)]
    [Column("voucher_code")]
    public string VoucherCode { get; set; }  // Mã voucher

    [StringLength(50)]
    [Column("voucher_name")]
    public string? VoucherName { get; set; }  // Tên voucher

    [Required]
    [StringLength(10)]
    [Column("voucher_type")]
    public string VoucherType { get; set; }  // Kiểu giảm giá ('percentage' hoặc 'fixed')

    [Required]
    [Column("discount_value", TypeName = "decimal(10,2)")]
    public decimal DiscountValue { get; set; }  // Giá trị giảm giá

    [Column("min_order_value", TypeName = "decimal(10,2)")]
    public decimal MinOrderValue { get; set; } = 0;  // Giá trị đơn hàng tối thiểu

    [Column("max_discount", TypeName = "decimal(10,2)")]
    public decimal? MaxDiscount { get; set; }  // Giới hạn giảm giá tối đa

    [Column("usage_limit")]
    public int? UsageLimit { get; set; }  // Tổng số lần sử dụng

    [Column("usage_count")]
    public int? UsageCount { get; set; } = 0;  // Số lần voucher đã được sử dụng

    [Required]
    [StringLength(20)]
    [Column("voucher_status")]
    public string VoucherStatus { get; set; } = "active";  // Trạng thái voucher (active/disable)

    [Required]
    [Column("start_date")]
    public DateTime StartDate { get; set; } // Ngày bắt đầu hiệu lực

    [Required]
    [Column("end_date")]
    public DateTime EndDate { get; set; }  // Ngày hết hạn

    [Required]
    [Column("created_date")]
    public DateTime CreatedDate { get; set; }  // Ngày tạo
}
