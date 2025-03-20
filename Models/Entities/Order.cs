using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Models.Entities;

[Table("Order")]
public partial class Order
{
    [Key]
    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("user_id")]
    [StringLength(450)]
    public string? UserId { get; set; }

    [Column("total", TypeName = "decimal(18, 2)")]
    public decimal Total { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string? Status { get; set; } // Pending, Processing, Shipping, Completed, Cancelled
                                        // Đang chờ xử lý, Đang xử lý, Đang giao hàng, Đã hoàn thành, Đã hủy

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Column("time_stamp", TypeName = "datetime")]
    public DateTime? TimeStamp { get; set; }

    [Column("payment_method")]
    public string? PaymentMethod { get; set; } // COD, VNPay

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [InverseProperty("Order")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [InverseProperty("Order")]
    public virtual ICollection<Return> Returns { get; set; } = new List<Return>();

    [ForeignKey("UserId")]
    [InverseProperty("Orders")]
    public virtual User? User { get; set; }
}
