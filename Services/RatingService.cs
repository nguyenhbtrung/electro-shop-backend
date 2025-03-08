using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
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
                    ProductId = p.ProductId,
                    UserId = p.UserId
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
                    ProductId = p.ProductId,
                    UserId = p.UserId,
                    RatingScore = p.RatingScore,
                    RatingContent = p.RatingContent,
                    Status = p.Status,
                    TimeStamp = p.TimeStamp,
                })
                .FirstOrDefaultAsync();
        }
        public async Task<RatingDto> CreateRatingAsync(CreateRatingRequestDto requestDto)
        {
            try
            {
                var rating = requestDto.ToRatingFromCreate();
                await _context.Ratings.AddAsync(rating);
                await _context.SaveChangesAsync();
                return rating.ToRatingDto();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<RatingDto> UpdateRatingAsync(int productId, UpdateRatingDto requestDto)
        {
            var rating = await _context.Ratings.FindAsync(productId);
            if (rating == null)
            {
                throw new NotFoundException("Rating not found");
            }
            rating.RatingScore = requestDto.RatingScore;
            rating.RatingContent = requestDto.RatingContent;
            rating.Status = requestDto.Status;
            rating.TimeStamp = requestDto.TimeStamp;
            await _context.SaveChangesAsync();
            return rating.ToRatingDto();
        }
        public async Task<bool> DeleteRatingAsync(int id)
        {
            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null)
            {
                throw new NotFoundException("Rating not found");
            }
            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();
            return true;
        }

        Task<RatingDto> IRatingService.DeleteRatingAsync(int productId)
        {
            throw new NotImplementedException();
        }
    }
}
