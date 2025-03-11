namespace electro_shop_backend.Models.DTOs.Product
{
    public class DiscountedProductDto
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
    }
}
