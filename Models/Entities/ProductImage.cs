using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Models.Entities;

[Table("Product_Image")]
public partial class ProductImage
{
    [Key]
    [Column("product_image_id")]
    public int ProductImageId { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [Column("image_url")]
    [StringLength(255)]
    public string? ImageUrl { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ProductImages")]
    public virtual Product? Product { get; set; }
}
