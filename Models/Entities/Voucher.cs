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
    [Column("voucher_id")]
    public int VoucherId { get; set; }

    [Column("voucher_code")]
    [StringLength(50)]
    public string? VoucherCode { get; set; }

    [Column("voucher_name")]
    [StringLength(100)]
    public string? VoucherName { get; set; }

    [Column("voucher_type")]
    [StringLength(50)]
    public string? VoucherType { get; set; }

    [Column("discount_value", TypeName = "decimal(18, 2)")]
    public decimal? DiscountValue { get; set; }

    [Column("min_order_value", TypeName = "decimal(18, 2)")]
    public decimal? MinOrderValue { get; set; }

    [Column("max_discount", TypeName = "decimal(18, 2)")]
    public decimal? MaxDiscount { get; set; }

    [Column("usage_limit")]
    public int? UsageLimit { get; set; }

    [Column("usage_count")]
    public int? UsageCount { get; set; }

    [Column("voucher_status")]
    [StringLength(50)]
    public string? VoucherStatus { get; set; }

    [Column("start_date", TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column("end_date", TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [Column("created_date", TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }
}
