using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Models.DTOs.ProductImage;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class ProductImageService:IProductImageService
    {
        private readonly ApplicationDbContext _context;
        public ProductImageService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProductImageDto>> GetProductImageAsync()
        {
            var productimage = await _context.ProductImages
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (productimage == null) return null;
            var productImageDto =ProductImageMapper.ToProductImageDto(productimage);
            return new List<ProductImageDto> { productImageDto };
        }
        public async Task<ProductImageDto> CreateProductImageAsync(CreateProductImageDto requestDto)
        {
            try
            {
                var productimage = requestDto.ToProductImageFromCreate();
                await _context.ProductImages.AddAsync(productimage);
                await _context.SaveChangesAsync();
                return productimage.ToProductImageDto();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeleteProductImageAsync(int id)
        {
            var productimage = await _context.ProductImages.FindAsync(id);
            if (productimage == null)
            {
                throw new NotFoundException("Không tìm thấy ảnh sản phẩm.");
            }
            _context.ProductImages.Remove(productimage);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
