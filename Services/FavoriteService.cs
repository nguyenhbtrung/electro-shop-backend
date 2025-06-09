using electro_shop_backend.Data;
using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.Favorite;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly ApplicationDbContext _context;
        public FavoriteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetFavoriteDTO>> GetAllFavoritesProduct(string userId)
        {
            var favorites = await _context.Favorites
                .Where(f => f.UserId == userId)
                .Include(f => f.Product)
                    .ThenInclude(p => p.ProductImages)
                .Include(f => f.Product.ProductDiscounts)
                    .ThenInclude(pd => pd.Discount)
                .AsNoTracking()
                .ToListAsync();

            var result = favorites.Select(f =>
            {
                var product = f.Product;

                var selectedAttributeDetailIds = new List<int>(); // để trống nếu không có phân loại
                var (originalPrice, discountedPrice, _, _) =
                    ProductCalculationValue.CalculateDiscount(product, selectedAttributeDetailIds);

                return new GetFavoriteDTO
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    ImageUrl = product.ProductImages?
                        .OrderBy(pi => pi.ProductImageId)
                        .Select(pi => pi.ImageUrl)
                        .FirstOrDefault() ?? string.Empty,
                    OriginalPrice = originalPrice,
                    DiscountedPrice = discountedPrice
                };
            });

            return result;

        }

        public async Task<bool> ToggleFavoriteAsync(string userId, int productId)
        {
            var existingFavorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

            if (existingFavorite != null)
            {
                _context.Favorites.Remove(existingFavorite);
                await _context.SaveChangesAsync();
                return false; // Đã xóa
            }
            else
            {
                var newFavorite = new Favorite
                {
                    UserId = userId,
                    ProductId = productId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Favorites.Add(newFavorite);
                await _context.SaveChangesAsync();
                return true; // Đã thêm
            }
        }
    }
}
