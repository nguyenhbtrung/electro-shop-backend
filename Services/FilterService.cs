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
        public async Task<List<ProductCardDto>> FilterProductsByAttributesAsync(
        int categoryId,
        int? priceFilter = null,   
        int? brandId = null,
        int? ratingFilter = null)  
        {
            var products = await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductImages)
                .Include(p => p.Categories)
                .Include(p => p.Ratings)
                .Include(p => p.ProductDiscounts)
                    .ThenInclude(pd => pd.Discount)
                .Include(p => p.Brand)
                .Where(p => p.Categories.Any(c => c.CategoryId == categoryId))
                .ToListAsync();

            decimal? minPrice = null;
            decimal? maxPrice = null;
            if (priceFilter.HasValue)
            {
                switch (priceFilter.Value)
                {
                    case 0: 
                        maxPrice = 5000000;
                        break;
                    case 1: 
                        minPrice = 5000000;
                        maxPrice = 10000000;
                        break;
                    case 2: 
                        minPrice = 10000000;
                        maxPrice = 15000000;
                        break;
                    case 3: 
                        minPrice = 15000000;
                        maxPrice = 20000000;
                        break;
                    case 4: 
                        minPrice = 20000000;
                        break;
                }
            }

            if (minPrice.HasValue || maxPrice.HasValue)
            {
                products = products.Where(product =>
                {
                    var (discountedPrice, discountType, discountValue) = ProductCalculationValue.CalculateDiscount(product);
                    bool meetsMin = !minPrice.HasValue || discountedPrice >= minPrice.Value;
                    bool meetsMax = !maxPrice.HasValue || discountedPrice <= maxPrice.Value;
                    return meetsMin && meetsMax;
                }).ToList();
            }

            if (brandId.HasValue)
            {
                products = products.Where(p => p.Brand != null && p.Brand.BrandId ==brandId.Value).ToList() ;
            }
            double? minRating = null;
            double? maxRating = null;
            if (ratingFilter.HasValue)
            {
                switch (ratingFilter.Value)
                {
                    case 0:
                        minRating = 0;
                        maxRating = 1;
                        break;
                    case 1:
                        minRating = 1;
                        maxRating = 2;
                        break;
                    case 2:
                        minRating = 2;
                        maxRating = 3;
                        break;
                    case 3:
                        minRating = 3;
                        maxRating = 4;
                        break;
                    case 4:
                        minRating = 4;
                        maxRating = 5;
                        break;
                }
            }
            if (minRating.HasValue && maxRating.HasValue)
            {
                products = products.Where(p =>
                    p.Ratings.Any() && p.Ratings.Average(r => r.RatingScore) >= minRating.Value &&
                    p.Ratings.Average(r => r.RatingScore) <= maxRating.Value).ToList();
            }
            var productDtos = products.Select(product =>
            {
                var (discountedPrice, discountType, discountValue) = ProductCalculationValue.CalculateDiscount(product);
                double avgRating = ProductCalculationValue.CalculateAverageRating(product);
                return new ProductCardDto
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
            }).ToList();

            return productDtos;
        }

    }
}
