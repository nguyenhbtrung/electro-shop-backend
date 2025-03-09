using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Models.DTOs.Product;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace electro_shop_backend.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<AllProductDto>> GetAllProductsIdsAndNamesAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Select(p => new AllProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name
                })
                .ToListAsync();
        }
        public async Task<ProductDto?> GetProductByIdAsync(int productId)
        {
            var product = await _context.Products
            .AsNoTracking()
            .Include(p => p.ProductImages)
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.ProductId == productId); 

            if (product == null) return null;
            var productDto = ProductMapper.ToProductDto(product);
            productDto.ProductImages = product.ProductImages
                .Select(ProductImageMapper.ToProductImageDto)
                .ToList();
            productDto.Categories = product.Categories
                .Select(CategoryMapper.ToCategoryIdDto)
                .ToList();
            return productDto;
        }
        public async Task<ProductDto> CreateProductAsync(CreateProductRequestDto requestDto)
        {
            try
            {
                var product = requestDto.ToProductFromCreate();
                if (requestDto.CategoryIds != null && requestDto.CategoryIds.Any())
                {
                    var categories = await _context.Categories
                        .Where(c => requestDto.CategoryIds.Contains(c.CategoryId))
                        .ToListAsync();
                    product.Categories = categories;
                }
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

