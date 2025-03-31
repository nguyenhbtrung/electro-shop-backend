using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Models.Entities;

[Table("Payment")]
public partial class Payment // Lưu lịch sử thanh toán
{
    [Key]
    [Column("payment_id")]
    public int PaymentId { get; set; }

    [Column("order_id")]
    public int? OrderId { get; set; }

    [Column("TransactionId")]
    public string? TransactionId { get; set; } // Mã giao dịch từ cổng thanh toán nếu thanh toán online

    [Column("amount", TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [Column("payment_url")]
    [StringLength(2048)]
    public string? PaymentUrl { get; set; }

    [Column("txn_ref")]
    public string? TxnRef { get; set; }

    [Column("payment_method")]
    public string? PaymentMethod { get; set; } // cod, vnpay

    [Column("payment_status")]
    [StringLength(50)]
    public string? PaymentStatus { get; set; } // pending, paid, failed

    [Column("created_time", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("paid_time", TypeName = "datetime")]
    public DateTime? PaidAt { get; set; }

    [Column("error_code")]
    public string? ErrorCode { get; set; }

    [Column("error_message")]
    public string? ErrorMessage { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Payments")]
    public virtual Order? Order { get; set; }
}
