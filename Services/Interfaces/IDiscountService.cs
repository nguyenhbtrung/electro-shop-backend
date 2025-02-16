using electro_shop_backend.Models.DTOs.Discount;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<DiscountDto> CreateDiscountAsync(CreateDiscountRequestDto requestDto);
    }
}
