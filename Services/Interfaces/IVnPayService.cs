using electro_shop_backend.Models.DTOs.Order;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(VnPayRequestDto request);

        VnPayResponseDto PaymentExcecute(IQueryCollection collection);
    }
}
