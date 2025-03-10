using electro_shop_backend.Models.DTOs.ReturnReason;
using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Models.Mappers
{
    public static class ReturnReasonMapper
    {
        public static ReturnReasonDto ToReturnReasonDto(this ReturnReason returnReasonEntity)
        {
            return new ReturnReasonDto
            {
                ReasonId = returnReasonEntity.ReasonId,
                Name = returnReasonEntity.Name
            };
        }
        public static ReturnReason CreateReturnReasonDto(this CreateReturnReasonRequestDto requestDto)
        {
            return new ReturnReason
            {
                Name = requestDto.Name
            };
        }
        public static void UpdateReturnReasonDto(this UpdateReturnReasonDto requestDto, ReturnReason returnReasonEntity)
        {
            returnReasonEntity.Name = requestDto.Name;
        }
    }
}
