using electro_shop_backend.Models.DTOs.Cart;

namespace electro_shop_backend.Services.Interfaces
{
    public class ICartService
    {
        Task<List<CartDto>> GetAllCartSyncs();

        Task<CartItemDto> AddToCartAsync(AddToCartDto requestDto);

        Task<List<CartItemDto>> GetCartByUserIdAsync(int userId);
        Task<List<CartItemDto>> GetCartByCartIdAsync(int cartId);

        Task<CartItemDto> UpdateCartItemAsync(int cartItemId, UpdateCartItemRequestDto requestDto);
        Task<CartItemDto> RemoveCartItemAsync(int cartItemId);
        Task<bool> DeleteCartItemAsync(int cartItemId);
    }
}
