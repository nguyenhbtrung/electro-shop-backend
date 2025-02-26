namespace electro_shop_backend.Models.DTOs.Cart
{
    public class CartDto
    {
        public int CartId { get; set; }

        public int UserId { get; set; }

        public DateTime TimeStamp { get; set; }

        public List<CartItemDto> CartItems { get; set; }
    }
}
