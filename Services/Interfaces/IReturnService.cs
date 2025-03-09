using electro_shop_backend.Models.DTOs.Return;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IReturnService
    {
        Task<List<AllReturnDto>> GetAllReturngAsync();
        Task<ReturnDto> CreateReturnAsync(int returnId, CreateReturnRequestDto requestDto);
        Task<ReturnDto> UpdateReturnAsync(int returnId, UpdateReturnDto requestDto);
        Task<bool> DeleteReturnAsync(int returnId);
    }
}
