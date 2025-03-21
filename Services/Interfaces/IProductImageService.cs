using electro_shop_backend.Models.DTOs.ProductImage;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IProductImageService
    {
        Task<List<ProductImageDto>> CreateProductImagesAsync(int productId, CreateProductImageDto requestDto);
        Task<ProductImageDto> UpdateProductImageAsync(int productId, CreateProductImageDto requestDto);
        Task<bool> DeleteProductImageAsync(int id);
    }
}
