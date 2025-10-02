using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace electro_shop_backend.Models.Entities
{
    [Table("Return_History")]
    public class ReturnHistory
    {
        [Key]
        [Column("return_history_id")]
        public int ReturnHistoryId { get; set; }
        [Column("return_id")]
        public int ReturnId { get; set; }

        [Column("status")]
        [StringLength(50)]
        public string? Status { get; set; }

        [Column("changed_at")]
        public DateTime? ChangedAt { get; set; }
        [ForeignKey("ReturnId")]
        [InverseProperty("ReturnHistories")]
        public virtual Return? Return { get; set; }

    }
}
