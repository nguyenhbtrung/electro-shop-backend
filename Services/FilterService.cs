using electro_shop_backend.Data;
using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.Product;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace electro_shop_backend.Services
{
    public class FilterService : IFilterService
    {
        private readonly ApplicationDbContext _context;
        public FilterService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductCardDto>> FindProductByNameAsync(string productName)
        {
            var products = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Categories)
                .Include(p => p.Ratings)
                .Include(p => p.ProductDiscounts)
                .ThenInclude(pd => pd.Discount)
                .Where(p => p.Name.ToLower().Contains(productName.ToLower()))
                .ToListAsync();
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
