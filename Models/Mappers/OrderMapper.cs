using electro_shop_backend.Models.DTOs.Order;
using electro_shop_backend.Models.Entities;

namespace electro_shop_backend.Models.Mappers
{
    public static class OrderMapper
    {
        public static AllOrderDto ToAllOrderDto(this Order order)
        {
            return new AllOrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                Total = order.Total,
                Status = order.Status,
                TimeStamp = order.TimeStamp
            };
        }

        public static OrderDto ToOrderDto(this Order order)
        {
            return new OrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                FullName = order.User!.FullName,
                Total = order.Total,
                PaymentMethod = order.PaymentMethod,
                PaymentStatus = order.Payments.FirstOrDefault(payment => payment.OrderId == order.OrderId)?.PaymentStatus,
                Status = order.Status,
                Address = order.Address,
                TimeStamp = order.TimeStamp,
                OrderItems = order.OrderItems
                .Select(OrderItem => new OrderItemDto
                {
                    OrderItemId = OrderItem.OrderItemId,
                    ProductId = OrderItem.ProductId,
                    Quantity = OrderItem.Quantity,
                    Price = OrderItem.Product?.Price ?? 0,
                    ProductName = OrderItem.Product?.Name,
                    ProductImage = OrderItem.Product?.ProductImages.FirstOrDefault()?.ImageUrl
                }).ToList(),
            };
        }
    }
}
