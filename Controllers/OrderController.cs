using System.Threading.Tasks;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Extensions;
using electro_shop_backend.Models.DTOs.Order;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;
        private readonly UserManager<User> _userManager;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger, UserManager<User> userManager)
        {
            _orderService = orderService;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet("admin/allorders")]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var listOrder = await _orderService.GetAllOrdersAsync();
            return Ok(listOrder);
        }

        [HttpGet("admin/vieworderbyid/{orderId}")]
        public async Task<IActionResult> GetOrderByOrderIdAsync(int orderId)
        {
            try
            {
                var order = await _orderService.GetOrderByOrderIdAsync(orderId);
                return Ok(order);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("user/vieworder")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetOrderByUserIdAsync()
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                var order = await _orderService.GetOrderByUserIdAsync(user.Id);
                return Ok(order);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("user/vieworderbystatus")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetOrderByStatusAsync(string status)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                var order = await _orderService.GetOrderByStatusAsync(user.Id, status);
                return Ok(order);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("user/createorder")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> CreateOrderAsync( string voucherCode = "", string payment = "")
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                var order = await _orderService.CreateOrderAsync(user.Id, voucherCode, payment);
                return Ok(order);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("user/updateorderaddress/{orderId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> UpdateOrderAddressAsync(OrderDto orderDto)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                var order = await _orderService.UpdateOrderAddressAsync(user.Id, orderDto);
                return Ok(order);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("admin/updateorderstatus/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatusAsync(int orderId, OrderDto orderDto)
        {
            try
            {
                var order = await _orderService.UpdateOrderStatusAsync(orderId, orderDto);
                return Ok(order);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("admin/cancelorder/{orderId}")]
        public async Task<IActionResult> CancelOrderAsync(int orderId)
        {
            try
            {
                var result = await _orderService.CancelOrderAsync(orderId);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("vnpay-callback")]
        public async Task<IActionResult> PaymentCallBack()
        {
            var result = await _orderService.HandlePaymentCallbackAsync(Request.Query);
            return Ok(result);
        }
    }
}
