using electro_shop_backend.Models.DTOs.Return;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IReturnService
    {
        Task<List<AllReturnDto>> GetAllReturnAsync();
        Task<ReturnDetailResponseDto> GetReturnByIdAsync(int returnId);
        Task<ReturnDetailAdminResponseDto> GetReturnByAdminAsync(int returnId);
        Task<List<ReturnUserHistoryDto>> GetUserReturnHistoryAsync(string userId);
        Task<CreateReturnResponseDto> CreateReturnAsync(string userId, CreateReturnRequestDto requestDto);
        Task<ReturnDto> UpdateReturnAsync(int returnId, UpdateReturnDto requestDto);
        Task<UpdateReturnStatusResponseDto> UpdateReturnStatusAsync(int returnId, UpdateReturnStatusRequestDto requestDto);
        Task<PaymentDTO> GetPaymentByOrderIdAsync(int orderId);
        Task<bool> DeleteReturnAsync(int returnId);
    }
}
