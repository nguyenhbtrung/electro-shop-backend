namespace electro_shop_backend.Models.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string? Info { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int? RatingCount { get; set; }
        public double? AverageRating { get; set; }
    }
}
