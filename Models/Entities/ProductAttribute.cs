using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace electro_shop_backend.Models.Entities 
{
    [Table("Product_Attribute")]
    public partial class ProductAttribute
    {
        [Key]
        public int AttributeId { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;
        public virtual ICollection<AttributeDetail> Details { get; set; } = new List<AttributeDetail>();
    }

}


