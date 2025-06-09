using electro_shop_backend.Data;
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
                .Select(f => new GetFavoriteDTO
                {
                    ProductId = f.Product.ProductId,
                    Name = f.Product.Name,
                    Price = f.Product.Price,
                    ImageUrl = f.Product.ProductImages != null
                        ? f.Product.ProductImages
                            .OrderBy(pi => pi.ProductImageId)
                            .Select(pi => pi.ImageUrl)
                            .FirstOrDefault() ?? string.Empty
                        : string.Empty
                })
                .ToListAsync();
            return favorites;
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
