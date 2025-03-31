namespace electro_shop_backend.Models.DTOs.Dashboard
{
    public class TopSellingProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Sold { get; set; }
        public decimal Price { get; set; } = 0;
    }
}
