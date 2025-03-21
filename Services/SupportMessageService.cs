using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Models.DTOs.SupportMessage;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class SupportMessageService : ISupportMessageService
    {
        private readonly ApplicationDbContext _context;

        public SupportMessageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateMessageResponseDto> CreateMessageAsync(string senderId, CreateMessageRequestDto requestDto)
        {
            var sender = await _context.Users.Select(u => new { u.Id }).FirstOrDefaultAsync(u => u.Id == senderId);
            if (sender == null)
            {
                throw new BadRequestException("Không tìm thấy người gửi");
            }
            if (requestDto.ReceiverId != null)
            {
                var receiver = await _context.Users.Select(u => new { u.Id }).FirstOrDefaultAsync(u => u.Id == requestDto.ReceiverId);
                if (receiver == null)
                {
                    throw new BadRequestException("Không tìm thấy người nhận");
                }
            }
            var newMessage = new SupportMessage
            {
                SenderId = senderId,
                ReceiverId = requestDto.ReceiverId,
                Message = requestDto.Message,
                IsFromAdmin = requestDto.IsFromAdmin
            };
            await _context.SupportMessages.AddAsync(newMessage);
            await _context.SaveChangesAsync();
            return new CreateMessageResponseDto
            {
                Id = newMessage.MessageId,
                SenderId = newMessage.SenderId,
                ReceiverId = newMessage.ReceiverId,
                Message = newMessage.Message,
                IsFromAdmin = newMessage.IsFromAdmin,
                SentAt = newMessage.SentAt
            };
        }

        public async Task<List<UserLatestMessageDto>> GetAllUserLatestMessagesAsync()
        {
            var dtos = await (
                from user in _context.Users
                    // join bảng UserRoles để lọc user có role là "User"
                join userRole in _context.UserRoles on user.Id equals userRole.UserId
                where userRole.RoleId == "User"
                // Lấy tin nhắn gần nhất liên quan đến user (nơi user là Sender hoặc Receiver)
                let latestMessage = _context.SupportMessages
                                        .Where(m => m.SenderId == user.Id || m.ReceiverId == user.Id)
                                        .OrderByDescending(m => m.SentAt)
                                        .FirstOrDefault()
                select new UserLatestMessageDto
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    // Nếu không có tin nhắn nào, các thuộc tính liên quan sẽ là null
                    IsFromAdmin = latestMessage != null ? latestMessage.IsFromAdmin : null,
                    SenderName = latestMessage != null && latestMessage.Sender != null ? latestMessage.Sender.UserName : null,
                    Message = latestMessage != null ? latestMessage.Message : null
                }
            ).ToListAsync();

            return dtos;
        }

        public async Task<List<SupportMessageDto>> GetMessagesByUserIdAsync(string userId)
        {
            var messages = await _context.SupportMessages
                .Include(m => m.Sender)
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .Select(m => new SupportMessageDto
                {
                    Id = m.MessageId,
                    SenderName = m.Sender!.UserName,
                    Message = m.Message,
                    IsFromAdmin = m.IsFromAdmin,
                    SentAt = m.SentAt
                })
                .OrderBy(m => m.SentAt)
                .ToListAsync();
            return messages;
        }
    }
}
