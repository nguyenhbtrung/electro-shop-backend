using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace electro_shop_backend.Models.Entities
{
    [Table("Support_Message")]
    public class SupportMessage
    {
        [Key]
        [Column("message_id")]
        public int MessageId { get; set; }

        [Column("sender_id")]
        [StringLength(450)]
        public string? SenderId { get; set; }

        [Column("receiver_id")]
        [StringLength(450)]
        public string? ReceiverId { get; set; }

        [Column("is_from_admin")]
        public bool IsFromAdmin { get; set; }

        [Column("sent_at", TypeName = "datetime")]
        public DateTime SentAt { get; set; }

        [ForeignKey("SenderId")]
        [InverseProperty("SentMessages")]
        public virtual User? Sender { get; set; }

        [ForeignKey("ReceiverId")]
        [InverseProperty("ReceivedMessages")]
        public virtual User? Receiver { get; set; }

    }
}
