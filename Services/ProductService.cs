using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Models.DTOs.Product;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
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
        public async Task<ProductDto> CreateProductAsync(CreateProductRequestDto requestDto)
        {
            try
            {
                var product = requestDto.ToProductFromCreate();
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return product.ToProductDto();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ProductDto> UpdateProductAsync(int productId, UpdateProductRequestDto requestDto)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new NotFoundException("Không tìm thấy sản phẩm.");
            }
            product.UpdateProductFromDto(requestDto);
            await _context.SaveChangesAsync();
            return product.ToProductDto();
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new NotFoundException("Không tìm thấy sản phẩm.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}

