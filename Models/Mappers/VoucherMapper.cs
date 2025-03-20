using electro_shop_backend.Models.DTOs.Voucher;
using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Models.Mappers
{
    public static class VoucherMapper
    {
        public static VoucherDto ToVoucherDto(this Voucher voucher) // Thông tin vouhcer
        {
            return new VoucherDto
            {
                VoucherId = voucher.VoucherId,
                VoucherCode = voucher.VoucherCode,
                VoucherName = voucher.VoucherName,
                VoucherType = voucher.VoucherType,
                DiscountValue = voucher.DiscountValue,
                MinOrderValue = voucher.MinOrderValue,
                MaxDiscount = voucher.MaxDiscount,
                UsageLimit = voucher.UsageLimit,
                UsageCount = voucher.UsageCount,
                VoucherStatus = voucher.VoucherStatus,
                StartDate = voucher.StartDate,
                EndDate = voucher.EndDate,
                CreatedDate = voucher.CreatedDate
            };
        }

        public static Voucher ToVoucherFromCreateVoucherDto(this CreateVoucherRequestDto requestDto) // Tạo voucher
        {
            DateTime utcVN = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "SE Asia Standard Time");
            return new Voucher
            {
                VoucherCode = requestDto.VoucherCode,
                VoucherName = requestDto.VoucherName,
                VoucherType = requestDto.VoucherType,
                DiscountValue = requestDto.DiscountValue,
                MinOrderValue = requestDto.MinOrderValue,
                MaxDiscount = requestDto.MaxDiscount,
                UsageLimit = requestDto.UsageLimit,
                VoucherStatus = requestDto.VoucherStatus,
                StartDate = requestDto.StartDate,
                EndDate = requestDto.EndDate,
                CreatedDate = utcVN
            };
        }

        public static void UpdateVoucherFromUpdateDto(this Voucher voucher, UpdateVoucherRequestDto requestDto) // Cập nhật voucher
        {
            voucher.VoucherCode = requestDto.VoucherCode;
            voucher.VoucherName = requestDto.VoucherName;
            voucher.VoucherType = requestDto.VoucherType;
            voucher.DiscountValue = requestDto.DiscountValue;
            voucher.MinOrderValue = requestDto.MinOrderValue;
            voucher.MaxDiscount = requestDto.MaxDiscount;
            voucher.UsageLimit = requestDto.UsageLimit;
            voucher.UsageCount = requestDto.UsageCount;
            voucher.VoucherStatus = requestDto.VoucherStatus;
            voucher.StartDate = requestDto.StartDate;
            voucher.EndDate = requestDto.EndDate;
        }
    }
}
