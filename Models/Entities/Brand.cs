using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.Entities
{
    [Table("Brand")]
    public class Brand
    {
        [Key]
        [Column("brand_id")]
        public int BrandId { get; set; }

        [Column("brand_name")]
        [StringLength(50)]
        public string? BrandName { get; set; }
        [Column("country")]
        [StringLength(200)]
        public string? Country { get; set; }
        [Column("image_url")]
        [StringLength(255)]
        public string? ImageUrl { get; set; }
        [Column("info")]
        [StringLength(1000)]
        public string? Info { get; set; }

        [InverseProperty("Brand")]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
