using electro_shop_backend.Models.DTOs.Discount;
using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Models.Mappers
{
    public static class DiscountMapper
    {
        public static Discount ToDiscountFromCreate(this CreateDiscountRequestDto requestDto)
        {
            return new Discount
            {
                Name = requestDto.Name,
                DiscountType = requestDto.DiscountType,
                DiscountValue = requestDto.DiscountValue,
                StartDate = requestDto.StartDate,
                EndDate = requestDto.EndDate
            };
        }

        public static DiscountDto ToDiscountDto(this Discount discount)
        {
            return new DiscountDto
            {
                DiscountId = discount.DiscountId,
                Name = discount.Name,
                DiscountType = discount.DiscountType,
                DiscountValue = discount.DiscountValue,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate
            };
        }
    }
}
