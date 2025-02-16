using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Models.Entities;

[Table("User")]
public partial class User : IdentityUser
{

    [Column("full_name")]
    [StringLength(100)]
    public string? FullName { get; set; }

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Column("avatar_img")]
    [StringLength(255)]
    public string? AvatarImg { get; set; }

    [Column("role")] // thua cot nay nay, xoa kieu gi
    [StringLength(50)]
    public string? Role { get; set; }

    [Column("user_status")]
    [StringLength(50)]
    public string? UserStatus { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [InverseProperty("User")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("User")]
    public virtual ICollection<ProductViewHistory> ProductViewHistories { get; set; } = new List<ProductViewHistory>();

    [InverseProperty("User")]
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
