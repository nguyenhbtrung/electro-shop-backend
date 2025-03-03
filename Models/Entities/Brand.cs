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

        [Key]
        [Column("brand_name")]
        [StringLength(50)]
        public string? BrandName { get; set; }

        [InverseProperty("Brand")]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
