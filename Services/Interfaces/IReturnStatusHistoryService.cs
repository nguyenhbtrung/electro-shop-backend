using electro_shop_backend.Models.DTOs.Return;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IReturnStatusHistoryService
    {
        Task<ReturnStatusHistoryDto> CreateReturnStatusHistoryAsync(CreateReturnStatusHistoryRequestDto requestDto);
    }
}
