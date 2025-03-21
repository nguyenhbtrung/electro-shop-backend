using electro_shop_backend.Models.DTOs.SupportMessage;

namespace electro_shop_backend.Services.Interfaces
{
    public interface ISupportMessageService
    {
        Task<CreateMessageResponseDto> CreateMessageAsync(string senderId, CreateMessageRequestDto requestDto);
        Task<List<UserLatestMessageDto>> GetAllUserLatestMessagesAsync();
        Task<List<SupportMessageDto>> GetMessagesByUserIdAsync(string userId);
    }
}
