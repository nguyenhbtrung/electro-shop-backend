using electro_shop_backend.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("ProductAttributeMapping")]
public class ProductAttributeMapping
{
    [Key, Column(Order = 0)]
    public int ProductId { get; set; }
    [Key, Column(Order = 1)]
    public int AttributeDetailId { get; set; }
    public virtual Product Product { get; set; } = null!;
    public virtual ProductAttributeDetail ProductAttributeDetail { get; set; } = null!;
}
