using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace electro_shop_backend.Models.Entities
{
    [Table("Notification")]
    public class Notification
    {
        [Key]
        [Column("noti_id")]
        public int NotiId { get; set; }

        [Column("user_id")]
        public string? UserId { get; set; }

        [Column("title")]
        [StringLength(50)]
        public string? Title { get; set; }

        [Column("content")]
        [StringLength(255)]
        public string Content { get; set; } = null!;

        [Column("type")]
        [StringLength(25)]
        public string? Type { get; set; }

        [Column("is_seen")]
        public bool IsSeen { get; set; } = false;

        [Column("link")]
        public string? Link { get; set; }

        [Column("create_at")]
        public DateTime CreateAt { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Notifications")]
        public virtual User? User { get; set; }

    }
}
