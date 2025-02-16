using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.Discount;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly ApplicationDbContext _context;

        public DiscountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DiscountDto> CreateDiscountAsync(CreateDiscountRequestDto requestDto)
        {
            try
            {
                var discount = requestDto.ToDiscountFromCreate();
                await _context.Discounts.AddAsync(discount);
                await _context.SaveChangesAsync();
                return discount.ToDiscountDto();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DiscountDto> GetDiscountByIdAsync(int discountId)
        {
            try
            {
                var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.DiscountId == discountId);
                if (discount == null)
                {
                    throw new NotFoundException("Không tìm thấy discount");
                }
                return discount.ToDiscountDto();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<DiscountDto>> GetDiscountsAsync(DiscountQuery discountQuery)
        {
            try
            {
                int skipNumber = (discountQuery.PageNumber - 1) * discountQuery.PageSize;
                var discounts = await _context.Discounts
                    .AsNoTracking()
                    .Select(d => new DiscountDto
                    {
                        DiscountId = d.DiscountId,
                        Name = d.Name,
                        DiscountType = d.DiscountType,
                        DiscountValue = d.DiscountValue,
                        StartDate = d.StartDate,
                        EndDate = d.EndDate
                    })
                    .OrderByDescending(d => d.StartDate)
                    .Skip(skipNumber)
                    .Take(discountQuery.PageSize)
                    .ToListAsync();

                return discounts;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DiscountDto> UpdateDiscountAsync(int discountId, CreateDiscountRequestDto requestDto)
        {
            try
            {
                var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.DiscountId == discountId);
                if (discount == null)
                {
                    throw new NotFoundException("Không tìm thấy discount");
                }
                discount.Name = requestDto.Name;
                discount.DiscountType = requestDto.DiscountType;
                discount.DiscountValue = requestDto.DiscountValue;
                discount.StartDate = requestDto.StartDate;
                discount.EndDate = requestDto.EndDate;
                await _context.SaveChangesAsync();
                return discount.ToDiscountDto();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
