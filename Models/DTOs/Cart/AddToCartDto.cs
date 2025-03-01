namespace electro_shop_backend.Models.DTOs.Cart
{
    public class AddToCartDto
    {
        public string? UserId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
