using electro_shop_backend.Models.DTOs.Favorite;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task<IEnumerable<GetFavoriteDTO>> GetAllFavoritesProduct(string userId);
        Task<bool> ToggleFavoriteAsync(string userId, int productId);
    }
}
