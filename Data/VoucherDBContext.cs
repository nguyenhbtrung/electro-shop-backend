using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace electro_shop_backend.Data;

[Table("Voucher")]
public class VoucherDBContext
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Tự động tăng
    public int VoucherId { get; set; }

    [Required]
    [StringLength(50)]
    public string VoucherCode { get; set; }  // Mã voucher

    [Required]
    [StringLength(50)]
    public string VoucherName { get; set; }  // Tên voucher

    [Required]
    [StringLength(10)]
    public string VoucherType { get; set; }  // Kiểu giảm giá ('percentage' hoặc 'fixed')

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal DiscountValue { get; set; }  // Giá trị giảm giá

    [Column(TypeName = "decimal(10,2)")]
    public decimal MinOrderValue { get; set; } = 0;  // Giá trị đơn hàng tối thiểu

    [Column(TypeName = "decimal(10,2)")]
    public decimal? MaxDiscount { get; set; }  // Giới hạn giảm giá tối đa

    public int? UsageLimit { get; set; }  // Tổng số lần sử dụng

    public int? UsageCount { get; set; }  // Số lần voucher đã được sử dụng

    [Required]
    [StringLength(20)]
    public string VoucherStatus { get; set; } = "active";  // Trạng thái voucher (active/disable)

    [Required]
    public DateTime StartDate { get; set; }  // Ngày bắt đầu hiệu lực

    [Required]
    public DateTime EndDate { get; set; }  // Ngày hết hạn

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreateDate { get; set; } = DateTime.Now;  // Ngày tạo
}

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    #region DbSet
    public DbSet<VoucherDBContext> Vouchers { get; set; }
    #endregion
}