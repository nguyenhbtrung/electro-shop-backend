namespace electro_shop_backend.Models.DTOs.Product
{
    public class ProductCardDto
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }

        public List<string> Images { get; set; } = new List<string>();

        public decimal OriginalPrice { get; set; }

        public decimal DiscountedPrice { get; set; }

        public string? DiscountType { get; set; }

        public decimal DiscountValue { get; set; }

        public double AverageRating { get; set; }
    }

}
