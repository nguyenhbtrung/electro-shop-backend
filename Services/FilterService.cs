using electro_shop_backend.Data;
using electro_shop_backend.Models.DTOs.Product;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace electro_shop_backend.Services
{
    public class FilterService:IFilterService
    {
        private readonly ApplicationDbContext _context;
        public FilterService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProductCardDto>> FindProductByNameAsync (string productName)
        {
            var products =await _context.Products
            .Include(p => p.ProductImages)
            .Include(p => p.Categories)
            .Include(p => p.Ratings)
            .Include(p => p.ProductDiscounts)
            .ThenInclude(pd => pd.Discount)
            .Where(p => p.Name.ToLower().Contains(productName.ToLower()))
            .ToListAsync();
            var now = DateTime.Now;
            var productDtos = products.Select(product =>
            {
                double avgRating = 0;
                if (product.Ratings != null && product.Ratings.Any())
                {
                    avgRating = product.Ratings.Average(r => r.RatingScore);
                }


                var effectiveDiscount = product.ProductDiscounts
                    .Where(pd => pd.Discount != null &&
                                 pd.Discount.StartDate <= now &&
                                 pd.Discount.EndDate >= now)
                    .Select(pd => pd.Discount)
                    .FirstOrDefault();

                string discountType = string.Empty;
                decimal discountValue = 0;
                decimal discountedPrice = product.Price;

                if (effectiveDiscount != null)
                {
                    discountType = effectiveDiscount.DiscountType;
                    discountValue = effectiveDiscount.DiscountValue ?? 0;

                    // Nếu discount theo phần trăm
                    if (string.Equals(discountType, "Percentage", StringComparison.OrdinalIgnoreCase))
                    {
                        discountedPrice = product.Price * (1 - discountValue / 100);
                    }
                    // Nếu discount theo số tiền cố định
                    else if (string.Equals(discountType, "Flat Amount", StringComparison.OrdinalIgnoreCase))
                    {
                        discountedPrice = product.Price - discountValue;
                    }

                    // Đảm bảo giá không âm
                    if (discountedPrice < 0)
                    {
                        discountedPrice = 0;
                    }
                }
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
