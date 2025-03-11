using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.Discount;
using electro_shop_backend.Models.DTOs.Product;
using electro_shop_backend.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<Discount> CreateDiscountAsync(CreateDiscountRequestDto requestDto);
        Task ApplyDiscountToProductsAsync(ApplyDiscountDto discountDto);
        Task<ICollection<DiscountDto>> GetDiscountsAsync(DiscountQuery discountQuery);
        Task<DiscountDto> GetDiscountByIdAsync(int discountId);
        Task<DiscountDto> UpdateDiscountAsync(int discountId, CreateDiscountRequestDto requestDto);
        Task DeleteDiscountAsync(int discountId);
        Task<DiscountProductsResponseDto> GetDiscountedProductsAsync(int discountId);

    }
}
