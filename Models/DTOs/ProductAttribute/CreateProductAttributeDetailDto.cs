namespace electro_shop_backend.Models.DTOs.ProductAttribute
{
    public class CreateProductAttributeDetailDto
    {
        public string Value { get; set; } = string.Empty;
        public decimal PriceModifier { get; set; }
    }
}
