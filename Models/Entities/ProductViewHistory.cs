using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Models.Entities;

[Table("Product_View_History")]
public partial class ProductViewHistory
{
    [Key]
    [Column("history_id")]
    public int HistoryId { get; set; }

    [Column("user_id")]
    [StringLength(450)]
    public string? UserId { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [Column("time_stamp", TypeName = "datetime")]
    public DateTime? TimeStamp { get; set; } = DateTime.Now;

    [ForeignKey("ProductId")]
    [InverseProperty("ProductViewHistories")]
    public virtual Product? Product { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("ProductViewHistories")]
    public virtual User? User { get; set; }
}
