using electro_shop_backend.Models.DTOs.ProductImage;

namespace electro_shop_backend.Models.DTOs.Cart
{
    public class CartItemDto
    {
        public int CartItemId { get; set; }

        public int? CartId { get; set; }

        public int? ProductId { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string? ProductName { get; set; }

        public string? ProductImage { get; set; }
    }
}
