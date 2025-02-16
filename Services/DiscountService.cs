using electro_shop_backend.Data;
using electro_shop_backend.Models.DTOs.Discount;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
                return discount.ToDiscountDto();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
