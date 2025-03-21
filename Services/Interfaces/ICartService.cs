using electro_shop_backend.Models.DTOs.Cart;

namespace electro_shop_backend.Services.Interfaces
{
    public interface ICartService
    {
        Task<List<CartDto>> GetAllCartASyncs();

        Task<CartItemDto> AddToCartAsync(string userId, int productId, int quantity);

        Task<List<UserCartDto>> GetCartByUserIdAsync(string userId);
        Task<List<CartItemDto>> GetCartByUserIdForAdminAsync(string userId);
        Task<List<CartItemDto>> GetCartByCartIdAsync(int cartId);

        Task<CartItemDto> UpdateCartItemQuantityAsync(string userid, int productId, int quantity);
        Task<CartItemDto> UpdateCartItemQuantityForAdminAsync(string userid, int productId, int quantity);

        Task<bool> DeleteCartAsync(string userId);
        Task<bool> DeleteCartItemAsync(string userId, int productId);

        Task<bool> DeleteCartAdminAsync(string userId);
        Task<bool> DeleteCartItemAdminAsync(string userId, int cartItemId);
    }
}
