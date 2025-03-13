using electro_shop_backend.Models.DTOs.Product;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IFilterService
    {
        Task<List<ProductCardDto>> FindProductByNameAsync(string productName, int n = 50);
        Task<List<ProductCardDto>> FilterProductsInCategoryAsync(
        int categoryId,
        int? priceFilter = null,
        int? brandId = null,
        int? ratingFilter = null);
        Task<List<ProductCardDto>> FilterProductsInBrandAsync(
        int brandId,
        int? priceFilter = null,
        int? categoryId = null,
        int? ratingFilter = null);
    }
}
