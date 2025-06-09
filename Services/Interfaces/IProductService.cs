using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.Discount;
using electro_shop_backend.Models.DTOs.Product;
using electro_shop_backend.Models.DTOs.ProductImage;
using electro_shop_backend.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<List<ProductCardDto>> GetAllProductsByUserAsync(ProductQuery productQuery, string? userId);
        Task<List<ProductCardDto>> GetDiscountedProductsAsync(ProductQuery productQuery, string? userId);
        Task<ProductDto?> GetProductByIdAsync(int productId, string? userId);
        Task<ProductDto> CreateProductAsync(CreateProductRequestDto requestDto);
        Task<ProductDto> UpdateProductAsync(int id,UpdateProductRequestDto requestDto);
        Task<bool> DeleteProductAsync(int id);
        Task<ProductGroupedDto> GetProductsByDiscountIdAsync(int? discountId, string? search);
        Task<List<ProductDto>> GetRecommendedProductsAsync(int productId);

    }
}
