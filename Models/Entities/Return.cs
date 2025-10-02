using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Models.Entities;

[Table("Return")]
public partial class Return
{
    [Key]
    [Column("return_id")]
    public int ReturnId { get; set; }

    [Column("order_id")]
    public int? OrderId { get; set; }

    [Column("reason")]
    public string? Reason { get; set; }

    [Column("detail")]
    public string? Detail { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("return_method")]
    [StringLength(100)]
    public string? ReturnMethod { get; set; }

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Column("admin_comment")]
    public string? AdminComment { get; set; }

    [Column("time_stamp")]
    public DateTime? TimeStamp { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Returns")]
    public virtual Order? Order { get; set; }

    [InverseProperty("Return")]
    public virtual ICollection<ReturnItem> ReturnItems { get; set; } = new List<ReturnItem>();

    [InverseProperty("Return")]
    public virtual ICollection<ReturnImage> ReturnImages { get; set; } = new List<ReturnImage>();
    [InverseProperty("Return")]
    public virtual ICollection<ReturnHistory> ReturnHistories { get; set; } = new List<ReturnHistory>();
}
