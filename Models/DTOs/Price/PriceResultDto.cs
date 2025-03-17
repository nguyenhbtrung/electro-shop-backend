namespace electro_shop_backend.Models.DTOs.Price
{
    public class PriceResultDto
    {
        public decimal OriginalPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public string DiscountType { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; }
    }
}
