namespace electro_shop_backend.Models.DTOs
{
    public class ProductAttributeDetailDto
    {
        public int AttributeDetailId { get; set; }
        public string Value { get; set; } = string.Empty;
        public decimal PriceModifier { get; set; }
        public int ProductAttributeId { get; set; }
        public string ProductAttributeName { get; set; } = string.Empty;

    }
}
