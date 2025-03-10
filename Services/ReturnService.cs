using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Models.DTOs.Rating;
using electro_shop_backend.Models.DTOs.Return;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class ReturnService : IReturnService
    {
        private readonly ApplicationDbContext _context;

        public ReturnService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AllReturnDto>> GetAllReturnAsync()
        {
            return await _context.Returns
                .AsNoTracking()
                .Select(p => new AllReturnDto
                {
                    ReturnId = p.ReturnId,
                    OrderId = p.OrderId,
                    Reason = p.Reason,
                    Detail = p.Detail,
                    Status = p.Status,
                    ReturnMethod = p.ReturnMethod,
                    Address = p.Address,
                    AdminComment = p.AdminComment,
                    TimeStamp = p.TimeStamp,
                })
                .ToListAsync();
        }

        public async Task<ReturnDto> CreateReturnAsync(int returnId, CreateReturnRequestDto requestDto)
        {
            try
            {
                var returnEntity = new Return
                {
                    ReturnId = returnId,
                    OrderId = requestDto.OrderId,
                    Reason = requestDto.Reason,
                    Detail = requestDto.Detail,
                    Status = requestDto.Status,
                    ReturnMethod = requestDto.ReturnMethod,
                    Address = requestDto.Address,
                    TimeStamp = requestDto.TimeStamp ?? DateTime.Now
                };
                await _context.Returns.AddAsync(returnEntity);
                await _context.SaveChangesAsync();
                return returnEntity.ToReturnDto();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ReturnDto> UpdateReturnAsync(int returnId, UpdateReturnDto requestDto)
        {
            var returnEntity = await _context.Returns
                .FirstOrDefaultAsync(r => r.ReturnId == returnId);
            if (returnEntity == null)
            {
                throw new NotFoundException("Not found");
            }

            await _context.SaveChangesAsync();
            return returnEntity.ToReturnDto();
        }

        public async Task<bool> DeleteReturnAsync(int returnId)
        {
            var returnEntity = await _context.Returns
                .FirstOrDefaultAsync(r => r.ReturnId == returnId);
            if (returnEntity == null)
            {
                throw new NotFoundException("Not found");
            }
            _context.Returns.Remove(returnEntity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
