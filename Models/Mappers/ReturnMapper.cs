using electro_shop_backend.Models.DTOs.Return;
using electro_shop_backend.Models.DTOs.ReturnReason;
using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Models.Mappers
{
    public static class ReturnMapper
    {
        public static ReturnDto ToReturnDto(this Return returnEntity)
        {
            return new ReturnDto
            {
                ReturnId = returnEntity.ReturnId,
                OrderId = returnEntity.OrderId,
                Reason = returnEntity.Reason,
                Detail = returnEntity.Detail,
                Status = returnEntity.Status,
                ReturnMethod = returnEntity.ReturnMethod,
                Address = returnEntity.Address,
                AdminComment = returnEntity.AdminComment,
                TimeStamp = returnEntity.TimeStamp
            };
        }
        //public static Return CreateReturnDto(this CreateReturnRequestDto requestDto)
        //{
        //    return new Return
        //    {
        //        OrderId = requestDto.OrderId,
        //        Reason = requestDto.Reason,
        //        Detail = requestDto.Detail,
        //        Status = requestDto.Status,
        //        ReturnMethod = requestDto.ReturnMethod,
        //        Address = requestDto.Address,
        //        TimeStamp = requestDto.TimeStamp
        //    };
        //}
        public static void UpdateReturnDto(this UpdateReturnDto requestDto, Return returnEntity)
        {
            returnEntity.OrderId = requestDto.OrderId;
            returnEntity.Reason = requestDto.Reason;
            returnEntity.Detail = requestDto.Detail;
            returnEntity.Status = requestDto.Status;
            returnEntity.ReturnMethod = requestDto.ReturnMethod;
            returnEntity.Address = requestDto.Address;
            returnEntity.TimeStamp = requestDto.TimeStamp;
        }
    }
}
