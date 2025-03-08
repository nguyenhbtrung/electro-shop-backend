using electro_shop_backend.Models.DTOs.Rating;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IRatingService
    {
        Task<List<AllRatingDto>> GetAllRatingAsync();
        Task<RatingDto?> GetRatingAsync(int ProductId);
        Task<RatingDto> CreateRatingAsync(string userId, CreateRatingRequestDto requestDto);
        Task<RatingDto> UpdateRatingAsync(int productId, UpdateRatingDto requestDto);
        Task<RatingDto> DeleteRatingAsync(int productId);
    }
}
