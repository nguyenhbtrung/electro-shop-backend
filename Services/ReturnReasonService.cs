using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Exceptions.CustomExceptions;
using electro_shop_backend.Models.DTOs.Return;
using electro_shop_backend.Models.DTOs.ReturnReason;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class ReturnReasonService : IReturnReasonService
    {
        private readonly ApplicationDbContext _context;

        public ReturnReasonService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<AllReturnReasonDto>> GetAllReturnReasonAsync()
        {
            return await _context.ReturnReasons
                .AsNoTracking()
                .Select(p => new AllReturnReasonDto
                {
                    ReasonId = p.ReasonId,
                    Name = p.Name,
                })
                .ToListAsync();
        }
        public async Task<ReturnReasonDto> CreateReturnReasonAsync(CreateReturnReasonRequestDto requestDto)
        {
            try
            {
                var reason = requestDto.CreateReturnReasonDto();
                await _context.ReturnReasons.AddAsync(reason);
                await _context.SaveChangesAsync();
                return reason.ToReturnReasonDto();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ReturnReasonDto> UpdateReturnReasonAsync(int reasonId, UpdateReturnReasonDto requestDto)
        {
            var returnReasonEntity = await _context.ReturnReasons
                .FindAsync(reasonId);
            if (returnReasonEntity == null)
            {
                throw new NotFoundException("Not found");
            }
            requestDto.UpdateReturnReasonDto(returnReasonEntity);
            await _context.SaveChangesAsync();
            return returnReasonEntity.ToReturnReasonDto();
        }
        public async Task DeleteReturnReasonAsync(int reasonId)
        {
            var returnReasonEntity = await _context.ReturnReasons
                .FirstOrDefaultAsync(r => r.ReasonId == reasonId);
            if (returnReasonEntity == null)
            {
                throw new NotFoundException("Not found");
            }
            _context.ReturnReasons.Remove(returnReasonEntity);
            await _context.SaveChangesAsync();
        }
    }
}
