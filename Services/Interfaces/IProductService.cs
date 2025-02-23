using electro_shop_backend.Models.DTOs.Product;
using electro_shop_backend.Models.DTOs.ProductImage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<AllProductDto>> GetAllProductsIdsAndNamesAsync();
        Task<ProductDto?> GetProductByIdAsync(int productId);
        Task<ProductDto> CreateProductAsync(CreateProductRequestDto requestDto);
        Task<ProductDto> UpdateProductAsync(int id,UpdateProductRequestDto requestDto);
        Task<bool> DeleteProductAsync(int id);

    }
}
