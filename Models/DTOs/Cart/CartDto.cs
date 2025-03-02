namespace electro_shop_backend.Models.DTOs.Cart
{
    public class CartDto
    {
        public int CartId { get; set; }

        public string? UserId { get; set; }

        public string? UserName { get; set; }

        public DateTime? TimeStamp { get; set; }

        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
    }
}
