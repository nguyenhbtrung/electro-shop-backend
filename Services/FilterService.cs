using electro_shop_backend.Data;
using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.Product;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace electro_shop_backend.Services
{
    public class FilterService : IFilterService
    {
        private readonly ApplicationDbContext _context;
        public FilterService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductCardDto>> FindProductByNameAsync(string productName, int n = 50)
        {
            if (string.IsNullOrEmpty(productName))
            {
                return new List<ProductCardDto>();
            }
            var processedQuery = productName.ToLower();
            processedQuery = Regex.Replace(processedQuery, "[^a-zA-Z0-9]", "");
            var products = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Ratings)
                .Include(p => p.ProductDiscounts)
                    .ThenInclude(pd => pd.Discount)
                .ToListAsync();
            var searchList = new List<ProductCardDto>();

            foreach (var product in products)
            {
                var processedName = product.Name.ToLower();
                processedName = Regex.Replace(processedName, "[^a-zA-Z0-9\\s]", "");

                if (processedName.Contains(processedQuery))
                {
                    var (discountedPrice, discountType, discountValue) = ProductCalculationValue.CalculateDiscount(product);
                    double avgRating = ProductCalculationValue.CalculateAverageRating(product);

                    var dto = new ProductCardDto
                    {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        Images = product.ProductImages?
                                    .Where(pi => !string.IsNullOrWhiteSpace(pi.ImageUrl))
                                    .Select(pi => pi.ImageUrl)
                                    .ToList() ?? new List<string>(),
                        OriginalPrice = product.Price,
                        DiscountedPrice = discountedPrice,
                        DiscountType = discountType,
                        DiscountValue = discountValue,
                        AverageRating = avgRating
                    };

                    searchList.Add(dto);
                }
            }

            return searchList.Take(n - 1).ToList();
        }
    }
}
