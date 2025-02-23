using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IProductViewHistoryService
    {
        Task<ProductViewHistory> CreateProductViewHistoryAsync(string userId, int productId);
    }
}
