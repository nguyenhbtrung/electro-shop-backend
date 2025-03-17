using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace electro_shop_backend.Models.Entities
{
    [Table("ProductAttributeDetail")]
    public class ProductAttributeDetail
    {
        [Key]
        public int AttributeDetailId { get; set; }
        [Required]
        [StringLength(100)]
        public string Value { get; set; } = null!;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PriceModifier { get; set; }

        [ForeignKey("ProductAttribute")]
        public int ProductAttributeId { get; set; }
        public virtual ProductAttribute ProductAttribute { get; set; } = null!;

    }

}
