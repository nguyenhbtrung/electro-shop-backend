using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.Discount;
using electro_shop_backend.Models.DTOs.ProductViewHistory;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
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

        public async Task DeleteAllProductViewHistoriesAsync(string userId)
        {
            try
            {
                var hitories = await _context.ProductViewHistories
                    .Where(h => h.UserId == userId)
                    .ToListAsync();
                _context.ProductViewHistories.RemoveRange(hitories);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteProductViewHistoryAsync(string userId, int productId)
        {
            try
            {
                var existedProduct = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductId == productId);
                if (existedProduct == null)
                {
                    throw new BadRequestException("Không tìm thấy sản phẩm");
                }
                var existedHistory = await _context.ProductViewHistories
                    .FirstOrDefaultAsync(h => h.UserId == userId && h.ProductId == productId);
                if (existedHistory == null)
                {
                    throw new NotFoundException("Không tìm thấy bản ghi");
                }
                _context.ProductViewHistories.Remove(existedHistory);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<ProductViewHistoryDto>> GetProductViewHistoriesAsync(string userId, HistoryQuery historyQuery)
        {
            try
            {
                int skipNumber = (historyQuery.PageNumber - 1) * historyQuery.PageSize;
                var histories = await _context.ProductViewHistories
                    .AsNoTracking()
                    .Where(h => h.UserId == userId)
                    .Select(h => new ProductViewHistoryDto
                    {
                        HistoryId = h.HistoryId,
                        UserId = h.UserId,
                        ProductId = h.ProductId,
                        TimeStamp = h.TimeStamp

                    })
                    .OrderByDescending(h => h.TimeStamp)
                    .Skip(skipNumber)
                    .Take(historyQuery.PageSize)
                    .ToListAsync();

                return histories;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductViewHistoryDto> UpdateProductViewHistoryAsync(string userId, int productId)
        {
            try
            {
                var existedProduct = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductId == productId);
                if (existedProduct == null)
                {
                    throw new BadRequestException("Không tìm thấy sản phẩm");
                }
                var existedHistory = await _context.ProductViewHistories
                    .FirstOrDefaultAsync(h => h.UserId == userId && h.ProductId == productId);
                if (existedHistory == null)
                {
                    throw new NotFoundException("Không tìm thấy bản ghi");
                }
                existedHistory.TimeStamp = DateTime.Now;
                await _context.SaveChangesAsync();
                return existedHistory.ToProductViewHistoryDto();
            }
            catch (Exception)
            {
                throw;
            }
            

        }
    }
}
