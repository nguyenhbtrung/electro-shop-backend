using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Models.Entities;

[Table("Return_Reason")]
public partial class ReturnReason
{
    [Key]
    [Column("reason_id")]
    public int ReasonId { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;
}
