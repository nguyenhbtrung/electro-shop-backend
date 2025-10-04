using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.Cart;
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
                    ProductImage = item.Product?.ProductImages.FirstOrDefault()?.ImageUrl,
                    ProductName = item.Product?.Name,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                }).ToList()
            };
        }

        public static UserCartDto ToUserCartDto(this CartItem cartitem)
        {
            if (cartitem.Product == null)
            {
                return new UserCartDto
                {
                    ProductId = cartitem.ProductId,
                    Price = 0,
                    DiscountedPrice = 0,
                    DiscountType = null,
                    DiscountValue = 0,
                    Quantity = cartitem.Quantity,
                    Stock = 0,
                    ProductName = null,
                    ProductImage = null
                };
            }

            var (originalPrice, discountedPrice, discountType, discountValue)
                = ProductCalculationValue.CalculateDiscount(cartitem.Product);

            return new UserCartDto
            {
                ProductId = cartitem.ProductId,
                Price = originalPrice,
                DiscountedPrice = discountedPrice,
                DiscountType = discountType,
                DiscountValue = discountValue,
                Quantity = cartitem.Quantity,
                Stock = cartitem.Product.Stock,
                ProductName = cartitem.Product.Name,
                ProductImage = cartitem.Product.ProductImages.FirstOrDefault()?.ImageUrl
            };
        }

        public static CartItemDto ToCartItemDto(this CartItem cartitem)
        {
            return new CartItemDto
            {
                CartId = cartitem.CartId,
                CartItemId = cartitem.CartItemId,
                ProductId = cartitem.ProductId,
                Quantity = cartitem.Quantity,
                ProductName = cartitem.Product?.Name,
                ProductImage = cartitem.Product?.ProductImages.FirstOrDefault()?.ImageUrl
            };
        }
    }
}
