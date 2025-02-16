using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Models.Entities;

[Table("Product_Discount")]
public partial class ProductDiscount
{
    [Key]
    [Column("product_discount_id")]
    public int ProductDiscountId { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [Column("discount_id")]
    public int? DiscountId { get; set; }

    [ForeignKey("DiscountId")]
    [InverseProperty("ProductDiscounts")]
    public virtual Discount? Discount { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ProductDiscounts")]
    public virtual Product? Product { get; set; }
}
