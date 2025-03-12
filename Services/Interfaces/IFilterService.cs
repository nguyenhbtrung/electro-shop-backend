using electro_shop_backend.Models.DTOs.Product;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IFilterService
    {
        Task<List<ProductCardDto>> FindProductByNameAsync(string productName, int n = 50);
        Task<List<ProductCardDto>> FilterProductsByAttributesAsync(
        int categoryId,
        int? priceFilter = null,
        string? brandName = null,
        int? ratingFilter = null);
    }
}
