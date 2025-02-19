﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Models.Entities;

public class VoucherModel
{
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
}
