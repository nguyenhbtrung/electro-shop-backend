using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.ProductViewHistory;
using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IProductViewHistoryService
    {
        Task<ProductViewHistory> CreateProductViewHistoryAsync(string userId, int productId);
        Task<ICollection<ProductViewHistoryDto>> GetProductViewHistoriesAsync(string userId);
        Task<ProductViewHistoryDto> UpdateProductViewHistoryAsync(string userId, int productId);
        Task DeleteProductViewHistoryAsync(string userId, int productId);
        Task DeleteAllProductViewHistoriesAsync(string userId);
    }
}
