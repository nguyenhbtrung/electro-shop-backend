using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Exceptions.CustomExceptions;
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
        private readonly DateTime utcVN = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "SE Asia Standard Time");
        private readonly ApplicationDbContext _context;
        private readonly IVnPayService _vnPayService;

        public OrderService(ApplicationDbContext context, IVnPayService vnPayService)
        {
            _context = context;
            _vnPayService = vnPayService;
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(order => order.OrderItems)
                .ThenInclude(OrderItem => OrderItem.Product)
                .ThenInclude(Product => Product.ProductImages)
                .Include(order => order.Payments)
                .Include(order => order.User)
                .OrderByDescending(h => h.TimeStamp)
                .Select(order => order.ToOrderDto())
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
                .Include(order => order.OrderItems)
                .ThenInclude(OrderItem => OrderItem.Product)
                .ThenInclude(Product => Product.ProductImages)
                .Include(order => order.Payments)
                .Include(order => order.User)
                .OrderByDescending(h => h.TimeStamp)
                .Select(order => order.ToOrderDto())
                .ToListAsync();
        }

        public async Task<List<OrderDto>> GetOrderByStatusAsync(string userId, string status)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(order => order.Status == status && order.UserId == userId)
                .Include(order => order.OrderItems)
                .Include(order => order.Payments)
                .Select(order => order.ToOrderDto())
                .ToListAsync();
        }

        public async Task<OrderDto> CreateOrderAsync(string userId, string voucherCode, string paymentmethod)
        {
            decimal totalPrice = 0;
            var cartitems = await _context.CartItems
                .Where(item => item.Cart!.UserId == userId)
                .Include(cartitem => cartitem.Product)
                .Include(cartitem => cartitem.Cart)
                .ToListAsync();

            var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);

            if (cartitems == null)
            {
                throw new NotFoundException("Cartitem not found");
            }

            var orderItems = new List<OrderItem>();

            foreach (var cartitem in cartitems)
            {
                if (cartitem.Quantity > cartitem.Product!.Stock)
                {
                    throw new BadRequestException("Product out of stock");
                }

                cartitem.Product!.Stock -= cartitem.Quantity;
                cartitem.Product.UnitsSold += cartitem.Quantity;
                OrderItem orderItem = new OrderItem
                {
                    ProductId = cartitem.ProductId,
                    ProductName = cartitem.Product!.Name,
                    Quantity = cartitem.Quantity,
                    Price = cartitem.Product!.Price,
                };
                totalPrice += orderItem.Price * orderItem.Quantity;
                cartitem.Cart!.CartItems.Remove(cartitem);
                _context.OrderItems.Add(orderItem);
                _context.Products.Update(cartitem.Product);
                orderItems.Add(orderItem);
            }

            decimal discountAmount = 0;
            if (!string.IsNullOrEmpty(voucherCode))
            {
                var voucher = await _context.Vouchers.FirstOrDefaultAsync(voucher => voucher.VoucherCode == voucherCode);

                if (voucher != null && totalPrice >= voucher.MinOrderValue)
                {
                    if (voucher.VoucherType == "percentage")
                    {
                        discountAmount = (totalPrice * voucher.DiscountValue) / 100;
                    }
                    else
                    {
                        discountAmount = voucher.DiscountValue;
                    }

                    voucher.UsageCount += 1;
                    if (voucher.UsageCount >= voucher.UsageLimit)
                    {
                        voucher.VoucherStatus = "disable";
                    }
                    _context.Vouchers.Update(voucher);
                }
            }

            decimal finalTotal = totalPrice - discountAmount;
            if (finalTotal < 0) finalTotal = 0;

            var order = new Order
            {
                UserId = userId,
                Total = finalTotal,
                Status = "pending",
                Address = user!.Address,
                TimeStamp = utcVN,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var request = new VnPayRequestDto
            {
                OrderId = order.OrderId,
            };

            var payment = new Payment
            {
                OrderId = order.OrderId,
                Amount = finalTotal,
                PaymentMethod = paymentmethod,
                PaymentStatus = "pending",
                CreatedAt = utcVN
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            string paymentUrl = null;
            if (paymentmethod == "cod")
            {
                order.PaymentMethod = "cod";
                payment.PaymentMethod = "cod";

                _context.Orders.Update(order);
                _context.Payments.Update(payment);
                await _context.SaveChangesAsync();
            }
            else if (paymentmethod == "vnpay")
            {
                paymentUrl = _vnPayService.CreatePaymentUrl(request);

                order.PaymentMethod = "vnpay";
                payment.PaymentMethod = "vnpay";

                _context.Orders.Update(order);
                _context.Payments.Update(payment);
                await _context.SaveChangesAsync();
            }
            else if (paymentmethod == "stripe")
            {
                paymentUrl = _vnPayService.CreatePaymentUrl(request);

                order.Status = "pending";
                payment.PaymentStatus= "successed";
                
                

                _context.Orders.Update(order);
                _context.Payments.Update(payment);
                await _context.SaveChangesAsync();
            }

            var orderDto = order.ToOrderDto();
            orderDto.PaymentUrl = paymentUrl;
            return orderDto;
        }

        public async Task<OrderDto> RePayment(int orderId)
        {
            Console.WriteLine(orderId);
            var order = await _context.Orders
                .Include(orderitem => orderitem.OrderItems)
                .Include(payment => payment.Payments)
                .FirstOrDefaultAsync(order => order.OrderId == orderId);

            var payment = await _context.Payments
                .FirstOrDefaultAsync(payment => payment.OrderId == orderId);

            var request = new VnPayRequestDto
            {
                OrderId = order!.OrderId,
            };

            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }

            string paymentUrl = null;
            if (order.PaymentMethod == "vnpay" && payment.PaymentStatus == "pending")
            {
                paymentUrl = _vnPayService.CreatePaymentUrl(request);
                _context.Entry(payment).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            var orderDto = order.ToOrderDto();
            orderDto.PaymentUrl = paymentUrl;
            return orderDto;
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

        public async Task<OrderDto> UpdateOrderStatusAsync(int orderId, string orderStatus)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(order => order.OrderId == orderId);
            var payment = await _context.Payments
                .FirstOrDefaultAsync(payment => payment.OrderId == orderId);

            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }

            order.Status = orderStatus;
            if (orderStatus == "successed")
            {
                payment.PaymentStatus = "paid";
            }
            else if (orderStatus == "cancelled" && payment.PaymentStatus == "paid")
            {
                payment.PaymentStatus = "refund";
            }
            await _context.SaveChangesAsync();
            return order.ToOrderUpdateDto();
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(order => order.OrderId == orderId);
            var payment = await _context.Payments
                .FirstOrDefaultAsync(payment => payment.OrderId == orderId);

            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }

            order.Status = "cancelled";
            if (payment == null)
            {
                throw new NotFoundException("Payment not found");
            }
            payment.PaymentStatus = "refund";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(orderitem => orderitem.OrderItems)
                .Include(payment => payment.Payments)
                .FirstOrDefaultAsync(order => order.OrderId == orderId);

            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }

            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Payments.RemoveRange(order.Payments);
            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<VnPayResponseDto> HandlePaymentCallbackAsync(IQueryCollection queryCollection)
        {
            var response = _vnPayService.PaymentExcecute(queryCollection);

            if (response.Success)
            {
                var order = await _context.Orders.FirstOrDefaultAsync(order => order.OrderId == response.OrderId);
                var payment = await _context.Payments.FirstOrDefaultAsync(payment => payment.OrderId == response.OrderId);
                if (order != null)
                {
                    payment.PaymentStatus = "paid";
                    payment.PaidAt = utcVN;
                    payment.TransactionId = response.TransactionId;
                    payment.TxnRef = response.Txn_Ref;
                    await _context.SaveChangesAsync();
                }
            }
            return response;
        }
    }
}
