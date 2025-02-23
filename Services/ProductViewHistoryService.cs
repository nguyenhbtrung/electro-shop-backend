using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class ProductViewHistoryService : IProductViewHistoryService
    {
        private readonly ApplicationDbContext _context;

        public ProductViewHistoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductViewHistory> CreateProductViewHistoryAsync(string userId, int productId)
        {
            try
            {
                var  existedProduct = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductId == productId);
                if (existedProduct == null)
                {
                    throw new BadRequestException("Không tìm thấy sản phẩm");
                }
                var existedHistory = await _context.ProductViewHistories
                    .FirstOrDefaultAsync(h => h.UserId == userId && h.ProductId == productId);
                if (existedHistory != null)
                {
                    throw new ConflictException("Bản ghi đã tồn tại");
                }
                var newHistory = new ProductViewHistory
                {
                    UserId = userId,
                    ProductId = productId
                };
                await _context.ProductViewHistories.AddAsync(newHistory);
                await _context.SaveChangesAsync();
                return newHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
