using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Models.Entities;

[Table("Product")]
public partial class Product
{
    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("info")]
    public string? Info { get; set; }

    [Column("price", TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Column("stock")]
    public int Stock { get; set; }

    [Column("brand_id")]
    public int? BrandId { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [InverseProperty("Product")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = new List<ProductDiscount>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductViewHistory> ProductViewHistories { get; set; } = new List<ProductViewHistory>();

    [InverseProperty("Product")]
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    [ForeignKey("ProductId")]
    [InverseProperty("Products")]
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    [ForeignKey("BrandId")]
    [InverseProperty("Products")]
    public virtual Brand? Brand { get; set; }
    public virtual ICollection<AttributeDetail> ProductAttributeDetails { get; set; } = new List<AttributeDetail>();

}
