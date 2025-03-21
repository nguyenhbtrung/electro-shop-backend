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
                    UserId = p.UserId,
                    RatingScore = p.RatingScore,
                    RatingContent = p.RatingContent,
                    TimeStamp = p.TimeStamp,
                })
                .ToListAsync();
        }
        public async Task<List<RatingDto>> GetRatingByProductIdAsync(int ProductId)
        {
            return await _context.Ratings
            .AsNoTracking()
                .Where(p => p.ProductId == ProductId)
                .Select(p => new RatingDto
                {
                    ProductId = p.ProductId,
                    UserId = p.UserId,
                    UserName = p.User.UserName,
                    RatingScore = p.RatingScore,
                    RatingContent = p.RatingContent,
                    Status = p.Status,
                    TimeStamp = p.TimeStamp,
                })
                .ToListAsync();
        }
        public async Task<List<RatingDto>> GetRatingByUserIdAsync(string userId)
        {
            return await _context.Ratings
            .AsNoTracking()
                .Where(p => p.UserId == userId)
                .Select(p => new RatingDto
                {
                    UserId = p.UserId,
                    UserName = p.User.UserName,
                    ProductId = p.ProductId,
                    RatingScore = p.RatingScore,
                    RatingContent = p.RatingContent,
                    Status = p.Status,
                    TimeStamp = p.TimeStamp,
                })
                .ToListAsync();
        }
        public async Task<RatingDto> CreateRatingAsync(string userId, CreateRatingRequestDto requestDto)
        {
            try
            {
                var rating = requestDto.ToRatingFromCreate(userId);
                await _context.Ratings.AddAsync(rating);
                await _context.SaveChangesAsync();
                return rating.ToRatingDto();
            }
            catch (Exception)
            {
                throw;
            }
        }

            public async Task<RatingDto> UpdateRatingAsync(int productId, UpdateRatingDto requestDto, string userId)
            {
                var rating = await _context.Ratings
                    .FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);
                if (rating == null)
                {
                    throw new NotFoundException("You can only change your own rating.");
                }
                rating.RatingScore = requestDto.RatingScore;
                rating.RatingContent = requestDto.RatingContent;
                rating.TimeStamp = requestDto.TimeStamp;

                await _context.SaveChangesAsync();
                return rating.ToRatingDto();
            }

            public async Task<bool> DeleteRatingAsync(int productId, string currentUserId, bool isAdmin = false)
            {
                // Nếu là admin thì bỏ qua kiểm tra UserId, còn không thì chỉ cho xóa đánh giá của chính người dùng.
                var rating = await _context.Ratings.FirstOrDefaultAsync(r => r.ProductId == productId
                                                  && (isAdmin || r.UserId == currentUserId));
                if (rating == null)
                {
                    throw new NotFoundException("Không tìm thấy đánh giá hoặc bạn không có quyền xóa đánh giá này.");
                }

                _context.Ratings.Remove(rating);
                await _context.SaveChangesAsync();
                return true;
            }
    }
}
