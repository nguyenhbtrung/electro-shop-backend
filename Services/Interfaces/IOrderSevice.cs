using electro_shop_backend.Models.DTOs.Order;

namespace electro_shop_backend.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllOrdersAsync();

        Task<OrderDto> GetOrderByOrderIdAsync(int orderId);
        Task<List<OrderDto>> GetOrderByUserIdAsync(string userId);
        Task<List<OrderDto>> GetOrderByStatusAsync(string userId, string status);

        Task<OrderDto> CreateOrderAsync(string userId, string voucherCode, string paymentmethod);
        Task<OrderDto> RePayment(int orderId);
        Task<OrderDto> UpdateOrderAddressAsync(string userId, OrderDto orderDto);
        Task<OrderDto> UpdateOrderStatusAsync(int orderId, string orderStatus);

        Task<bool> CancelOrderAsync(int orderId);
        Task<bool> DeleteOrderAsync(int orderId);
        Task<VnPayResponseDto> HandlePaymentCallbackAsync(IQueryCollection queryCollection);
    }
}
