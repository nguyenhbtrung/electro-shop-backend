using electro_shop_backend.Models.DTOs.Cart;
using electro_shop_backend.Models.DTOs.Voucher;
using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Models.Mappers
{
    public static class CartMapper
    {
        public static CartDto ToCartDto(this Cart cart) 
        {
            return new CartDto
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                UserName = cart.User?.UserName,
                TimeStamp = cart.TimeStamp,
                CartItems = cart.CartItems
                .Select(item => new CartItemDto
                {
                    CartItemId = item.CartItemId,
                    CartId = item.CartId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                }).ToList()
            };
        }

        public static CartItemDto ToCartItemDto(this CartItem cartitem) 
        {
            return new CartItemDto
            {
                CartId = cartitem.CartId,
                CartItemId = cartitem.CartItemId,
                ProductId = cartitem.ProductId,
                Quantity = cartitem.Quantity
            };
        }
    }
}
