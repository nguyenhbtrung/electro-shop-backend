using electro_shop_backend.Models.DTOs.Product;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IFilterService
    {
        Task<List<ProductCardDto>> FindProductByNameAsync(string productName, int n = 50);
    }
}
