using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Models.DTOs.Order;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace electro_shop_backend.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AllOrderDto>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .AsNoTracking()
                .Select(OrderDto => OrderDto.ToAllOrderDto())
                .ToListAsync();
        }

        public async Task<OrderDto> GetOrderByOrderIdAsync(int orderId)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .Include(orderitem => orderitem.OrderItems)
                .FirstOrDefaultAsync(order => order.OrderId == orderId);
            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }
            return order.ToOrderDto();
        }

        public async Task<List<OrderDto>> GetOrderByUserIdAsync(string userId)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(order => order.UserId == userId)
                .Include(orderitem => orderitem.OrderItems)
                .Select(order => order.ToOrderDto())
                .ToListAsync();
        }

        public async Task<OrderDto> CreateOrderAsync(string userId, List<int> selectedProductIds, string voucherCode)
        {
            decimal totalPrice = 0;
            var cartitems = await _context.CartItems
                .Where(item => selectedProductIds.Contains(item.ProductId ?? 0) && item.Cart!.UserId == userId)
                .Include(cartitem => cartitem.Product)
                .Include(cartitem => cartitem.Cart)
                .ToListAsync();

            if (cartitems == null)
            {
                throw new NotFoundException("Cartitem not found");
            }

            foreach (var cartitem in cartitems)
            {
                if (cartitem.Quantity > cartitem.Product!.Stock)
                {
                    throw new BadRequestException("Product out of stock");
                }

                cartitem.Product!.Stock -= cartitem.Quantity;

                OrderItem orderItem = new OrderItem
                {
                    ProductId = cartitem.ProductId,
                    ProductName = cartitem.Product!.Name,
                    Quantity = cartitem.Quantity,
                    Price = cartitem.Product!.Price,
                };
                totalPrice =+orderItem.Price * orderItem.Quantity;
                cartitem.Cart!.CartItems.Remove(cartitem);
                _context.OrderItems.Add(orderItem);
                _context.Products.Update(cartitem.Product);
            }

            decimal discountAmount = 0;
            if (!string.IsNullOrEmpty(voucherCode))
            {
                var voucher = await _context.Vouchers.FirstOrDefaultAsync(voucher => voucher.VoucherCode == voucherCode);
                if (voucher != null && totalPrice >= voucher.MinOrderValue)
                {
                    if (voucher.VoucherType == "Percentage")
                    {
                        discountAmount = (totalPrice * voucher.DiscountValue) / 100;
                    }
                    else
                    {
                        discountAmount = voucher.DiscountValue;
                    }
                }
            }

            decimal finalTotal = totalPrice - discountAmount;
            if (finalTotal < 0) finalTotal = 0;

            var order = new Order
            {
                UserId = userId,
                Total = finalTotal,
                Status = "Pending",
                TimeStamp = DateTime.UtcNow,
                //OrderItems = orderItems
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order.ToOrderDto();
        }

        public async Task<OrderDto> UpdateOrderAddressAsync(string userId, OrderDto orderDto)
        {
            var order = await _context.Orders
                .Include(orderitem => orderitem.OrderItems)
                .FirstOrDefaultAsync(order => order.UserId == userId);

            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }

            order.Address = orderDto.Address;
            await _context.SaveChangesAsync();
            return order.ToOrderDto();
        }

        public async Task<OrderDto> UpdateOrderStatusAsync(int orderId, OrderDto orderDto)
        {
            var order = await _context.Orders
                .Include(orderitem => orderitem.OrderItems)
                .FirstOrDefaultAsync(order => order.OrderId == orderId);

            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }

            order.Status = orderDto.Status;
            await _context.SaveChangesAsync();
            return order.ToOrderDto();
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(order => order.OrderId == orderId);

            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }

            order.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
