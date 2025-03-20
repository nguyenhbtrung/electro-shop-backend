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
            return new CreateMessageResponseDto
            {
                SenderId = newMessage.SenderId,
                ReceiverId = newMessage.ReceiverId,
                Message = newMessage.Message,
                IsFromAdmin = newMessage.IsFromAdmin,
                SentAt = newMessage.SentAt
            };
        }
    }
}
