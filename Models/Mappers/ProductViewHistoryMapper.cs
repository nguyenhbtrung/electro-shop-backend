using electro_shop_backend.Models.DTOs.ProductViewHistory;
using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Models.Mappers
{
    public static class ProductViewHistoryMapper
    {
        public static ProductViewHistoryDto ToProductViewHistoryDto(this ProductViewHistory productViewHistory)
        {
            return new ProductViewHistoryDto
            {
                HistoryId = productViewHistory.HistoryId,
                UserId = productViewHistory.UserId,
                ProductId = productViewHistory.ProductId
            };
        }
    }
}
