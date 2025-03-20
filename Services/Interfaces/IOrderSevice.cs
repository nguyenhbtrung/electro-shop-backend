using electro_shop_backend.Models.DTOs.Order;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<AllOrderDto>> GetAllOrdersAsync();

        Task<OrderDto> GetOrderByOrderIdAsync(int orderId);
        Task<List<OrderDto>> GetOrderByUserIdAsync(string userId);

        Task<OrderDto> CreateOrderAsync(string userId, List<int> selectedCardItemIds, string voucherCode, string paymentmethod);

        Task<OrderDto> UpdateOrderAddressAsync(string userId, OrderDto orderDto);
        Task<OrderDto> UpdateOrderStatusAsync(int orderId, OrderDto orderDto);

        Task<bool> CancelOrderAsync(int orderId);
        Task<VnPayResponseDto> HandlePaymentCallbackAsync(IQueryCollection queryCollection);
    }
}
