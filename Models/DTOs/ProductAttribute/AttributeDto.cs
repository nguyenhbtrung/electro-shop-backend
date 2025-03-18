namespace electro_shop_backend.Models.DTOs.ProductAttribute
{
    public class AttributeDto
    {
        public int AttributeDetailId { get; set; }
        public string Value { get; set; } = string.Empty;
        public decimal PriceModifier { get; set; }
        public int ProductAttributeId { get; set; }
    }
}
