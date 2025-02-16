using electro_shop_backend.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductIdsAndNamesAsync();
        Task<ProductDto?> GetProductByIdAsync(int productId);
    }
}
