using electro_shop_backend.Models.DTOs.Voucher;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IVoucherService
    {
        Task<List<VoucherDto>> GetAllVouchersAsyncs();
        Task<List<VoucherDto>> GetVoucherAvailableAsync();
        Task<VoucherDto?> GetVoucherByIdAsyncs(int voucherId);
        Task<VoucherDto> CreateVoucherAsync(CreateVoucherRequestDto requestDto);
        Task<VoucherDto> UpdateVoucherAsync(int voucherId, UpdateVoucherRequestDto requestDto);
        Task<bool> DeleteVoucherAsync(int voucherId);

        Task CheckAndUpdateVoucherStatusAsync();
    }
}
