using electro_shop_backend.Data;
using electro_shop_backend.Models.DTOs.Return;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Exceptions.CustomExceptions;

namespace electro_shop_backend.Services
{
    public class ReturnStatusHistoryService :IReturnStatusHistoryService
    {
        private readonly ApplicationDbContext _context;

        public ReturnStatusHistoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReturnStatusHistoryDto> CreateReturnStatusHistoryAsync(CreateReturnStatusHistoryRequestDto requestDto)
        {
            var existingReturn = await _context.Returns
                .Select(r => new { r.ReturnId})
                .FirstOrDefaultAsync(r => r.ReturnId == requestDto.ReturnId) ?? 
                throw new NotFoundException("Không tìm thấy Yêu cầu hoàn trả");
            var newStatusHistory = new ReturnHistory
            {
                ReturnId = requestDto.ReturnId,
                Status = requestDto.Status.ToString().ToLower(),
                ChangedAt = requestDto.ChangedAt
            };
            await _context.ReturnHistories.AddAsync(newStatusHistory);
            await _context.SaveChangesAsync();
            return newStatusHistory.ToReturnStatusHistoryDto();
        }
    }
}
