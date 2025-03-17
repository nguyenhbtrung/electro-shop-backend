using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.Entities
{
    [Table("AttributeDetail")]
    public class AttributeDetail
    {
        [Key]
        public int AttributeDetailId { get; set; }
        [Required]
        [StringLength(100)]
        public string Value { get; set; } = null!;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PriceModifier { get; set; }

        [ForeignKey("ProductAttributeId")]
        public virtual ProductAttribute ProductAttribute { get; set; } = null!;
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    }

}
