using electro_shop_backend.Data;
using electro_shop_backend.Models.DTOs.Rating;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class RatingService : IRatingService
    {
        private readonly ApplicationDbContext _context;

        public RatingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AllRatingDto>> GetAllRatingAsync()
        {
            return await _context.Ratings
                .AsNoTracking()
                .Select(p => new AllRatingDto
                {
                    UserId = p.UserId,
                    ProductId = p.ProductId
                })
                .ToListAsync();
        }
        public async Task<RatingDto?> GetRatingAsync(int ProductId)
        {
            return await _context.Ratings
            .AsNoTracking()
                .Where(p => p.ProductId == ProductId)
                .Select(p => new RatingDto
                {
                    UserId = p.UserId,
                    ProductId = p.ProductId,
                    RatingScore = p.RatingScore,
                    RatingContent = p.RatingContent,
                    Status = p.Status,
                    TimeStamp = p.TimeStamp,
                })
                .FirstOrDefaultAsync();
        }
    }
}
