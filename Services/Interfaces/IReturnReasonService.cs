using electro_shop_backend.Models.DTOs.ReturnReason;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IReturnReasonService
    {
        Task<List<AllReturnReasonDto>> GetAllReturnReasonAsync();
        Task<ReturnReasonDto> CreateReturnReasonAsync(CreateReturnReasonRequestDto requestDto);
        Task<ReturnReasonDto> UpdateReturnReasonAsync(int reasonId, UpdateReturnReasonDto requestDto);
        Task DeleteReturnReasonAsync(int reasonId);
    }
}
