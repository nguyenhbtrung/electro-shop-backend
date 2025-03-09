using electro_shop_backend.Models.DTOs.Rating;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IRatingService
    {
        Task<List<AllRatingDto>> GetAllRatingAsync();
        Task<List<RatingDto>> GetRatingByProductIdAsync(int ProductId);
        Task<List<RatingDto>> GetRatingByUserIdAsync(string userId);
        Task<RatingDto> CreateRatingAsync(string userId, CreateRatingRequestDto requestDto);
        Task<RatingDto> UpdateRatingAsync(int productId, UpdateRatingDto requestDto, string userId);
        Task<bool> DeleteRatingAsync(int productId, string userId);
    }
}
