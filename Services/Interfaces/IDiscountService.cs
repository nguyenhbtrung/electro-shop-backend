using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.Discount;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<DiscountDto> CreateDiscountAsync(CreateDiscountRequestDto requestDto);
        Task<ICollection<DiscountDto>> GetDiscountsAsync(DiscountQuery discountQuery);
        Task<DiscountDto> GetDiscountByIdAsync(int discountId);
        
    }
}
