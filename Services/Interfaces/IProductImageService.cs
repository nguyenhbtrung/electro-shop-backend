using electro_shop_backend.Models.DTOs.ProductImage;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IProductImageService
    {
        Task<List<ProductImageDto>> GetProductImageAsync();
        Task<ProductImageDto> CreateProductImageAsync(CreateProductImageDto requestDto);
        Task<bool> DeleteProductImageAsync(int id);
    }
}
