using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Hubs;
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
            var sender = await _context.Users.Select(u => new { u.Id, u.UserName }).FirstOrDefaultAsync(u => u.Id == senderId);
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
                SenderName = sender.UserName,
                ReceiverId = newMessage.ReceiverId,
                Message = newMessage.Message,
                IsFromAdmin = newMessage.IsFromAdmin,
                SentAt = newMessage.SentAt
            };
        }

        public async Task<List<UserLatestMessageDto>> GetAllUserLatestMessagesAsync()
        {
            // Lấy dữ liệu từ DB với tin nhắn gần nhất của mỗi user có role "User"
            var userMessages = await (
                from user in _context.Users
                join userRole in _context.UserRoles on user.Id equals userRole.UserId
                where userRole.RoleId == "User"
                let latestMessage = _context.SupportMessages
                                        .Include(m => m.Sender)
                                        .Where(m => m.SenderId == user.Id || m.ReceiverId == user.Id)
                                        .OrderByDescending(m => m.SentAt)
                                        .FirstOrDefault()
                select new
                {
                    User = user,
                    LatestMessage = latestMessage
                }
            ).ToListAsync();

            // Map sang DTO, chuyển đổi trạng thái claim từ ConversationLocks thành AdminId
            var dtos = userMessages.Select(um => new UserLatestMessageDto
            {
                UserId = um.User.Id,
                UserName = um.User.UserName,
                FullName = um.User.FullName,
                IsFromAdmin = um.LatestMessage != null ? (bool?)um.LatestMessage.IsFromAdmin : null,
                SenderName = um.LatestMessage != null && um.LatestMessage.Sender != null ? um.LatestMessage.Sender.UserName : null,
                Message = um.LatestMessage != null ? um.LatestMessage.Message : null,
                // Lấy adminId từ ConversationLocks, nếu có admin đang claim conversation của user này
                AdminId = ChatHub.ConversationLocks.TryGetValue(um.User.Id, out string adminId) ? adminId : null
            }).ToList();

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
