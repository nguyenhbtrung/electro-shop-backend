using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using electro_shop_backend.Models.DTOs.Rating;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Models.Entities;

[PrimaryKey("UserId", "ProductId")]
[Table("Rating")]
public partial class Rating
{
    [Key]
    [Column("user_id")]
    public string UserId { get; set; } = null!;

    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("rating_score")]
    public int RatingScore { get; set; }

    [Column("rating_content")]
    public string? RatingContent { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("time_stamp", TypeName = "datetime")]
    public DateTime? TimeStamp { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Ratings")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Ratings")]
    public virtual User User { get; set; } = null!;

    internal RatingDto ToRatingDto()
    {
        throw new NotImplementedException();
    }
}
