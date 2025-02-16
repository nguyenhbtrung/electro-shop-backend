using electro_shop_backend.Data;
using electro_shop_backend.Models.DTOs;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace electro_shop_backend.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> GetAllProductIdsAndNamesAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name
                })
                .ToListAsync();
        }

        public async Task<ProductDto?> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.ProductId == productId)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Info = p.Info,
                    Price = p.Price,
                    Stock = p.Stock,
                    RatingCount = p.RatingCount,
                    AverageRating = p.AverageRating
                })
                .FirstOrDefaultAsync();
        }
    }
}

