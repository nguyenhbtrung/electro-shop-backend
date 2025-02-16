using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Models.Entities;

[Table("Return_Item")]
public partial class ReturnItem
{
    [Key]
    [Column("return_item_id")]
    public int ReturnItemId { get; set; }

    [Column("return_id")]
    public int? ReturnId { get; set; }

    [Column("order_item_id")]
    public int? OrderItemId { get; set; }

    [Column("return_quantity")]
    public int ReturnQuantity { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [ForeignKey("OrderItemId")]
    [InverseProperty("ReturnItems")]
    public virtual OrderItem? OrderItem { get; set; }

    [ForeignKey("ReturnId")]
    [InverseProperty("ReturnItems")]
    public virtual Return? Return { get; set; }
}
