namespace electro_shop_backend.Models.DTOs.Cart
{
    public class UserCartDto
    {
        public int? ProductId { get; set; }

        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public string DiscountType { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; }

        public int Quantity { get; set; }

        public int Stock { get; set; }

        public string? ProductName { get; set; }

        public string? ProductImage { get; set; }
    }
}
