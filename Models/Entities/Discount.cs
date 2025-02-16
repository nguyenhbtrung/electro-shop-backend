using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Models.Entities;

[Table("Discount")]
public partial class Discount
{
    [Key]
    [Column("discount_id")]
    public int DiscountId { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("discount_type")]
    [StringLength(50)]
    public string? DiscountType { get; set; }

    [Column("discount_value", TypeName = "decimal(18, 2)")]
    public decimal? DiscountValue { get; set; }

    [Column("start_date", TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column("end_date", TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [InverseProperty("Discount")]
    public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = new List<ProductDiscount>();
}
