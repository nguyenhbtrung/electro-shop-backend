using electro_shop_backend.Models.DTOs.ProductImage;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IProductImageService
    {
        Task<ProductImageDto> CreateProductImageAsync(int id,CreateProductImageDto requestDto);
        Task<bool> DeleteProductImageAsync(int id);
        Task<ProductImageDto> UpdateProductImageAsync(int productId, CreateProductImageDto requestDto);
    }
}
