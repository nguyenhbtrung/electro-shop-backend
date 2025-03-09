using electro_shop_backend.Models.DTOs.Return;
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
    }
}
