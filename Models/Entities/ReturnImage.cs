using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace electro_shop_backend.Models.Entities
{
    [Table("Return_Image")]
    public class ReturnImage
    {
        [Key]
        [Column("return_image_id")]
        public int ReturnImageId { get; set; }

        [Column("return_id")]
        public int ReturnId { get; set; }

        [Column("image_url")]
        [StringLength(255)]
        public string? ImageUrl { get; set; }

        [ForeignKey("ReturnId")]
        [InverseProperty("ReturnImages")]
        public virtual Return? Return { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
    }
}
