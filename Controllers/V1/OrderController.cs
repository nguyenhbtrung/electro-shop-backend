using System.Threading.Tasks;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Extensions;
using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.Order;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger, UserManager<User> userManager, IConfiguration configuration)
        {
            _orderService = orderService;
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
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
        public async Task<IActionResult> CreateOrderAsync(string voucherCode = "", string payment = "")
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

        [HttpPut("user/repayment/{orderId}")]
        public async Task<IActionResult> RePaymentAsync(int orderId)
        {
            try
            {
                var order = await _orderService.RePayment(orderId);
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

        [HttpPut("admin/updateorderstatus")]
        public async Task<IActionResult> UpdateOrderStatusAsync(int orderId, string orderStatus)
        {
            try
            {
                var order = await _orderService.UpdateOrderStatusAsync(orderId, orderStatus);
                return Ok(order);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("admin/cancelorder/{orderId}")]
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

        [HttpDelete("admin/deleteorder/{orderId}")]
        public async Task<IActionResult> DeleteOrderAsync(int orderId)
        {
            try
            {
                var result = await _orderService.DeleteOrderAsync(orderId);
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
            string vnp_HashSecret = _configuration["VnPay:HashSecret"] ?? string.Empty;
            var vnpParams = HttpContext.Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
            VnPayLibrary vnpay = new VnPayLibrary();

            // Thêm tất cả tham số bắt đầu bằng "vnp_"
            foreach (var s in vnpParams)
            {
                if (!string.IsNullOrEmpty(s.Key) && s.Key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(s.Key, s.Value);
                }
            }

            string orderId = vnpay.GetResponseData("vnp_TxnRef");
            string vnpayTranId = vnpay.GetResponseData("vnp_TransactionNo");
            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            string vnp_SecureHash = vnpParams["vnp_SecureHash"];
            string TerminalID = vnpParams["vnp_TmnCode"];
            long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
            string bankCode = vnpParams["vnp_BankCode"];
            string payDate = vnpParams["vnp_PayDate"];

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

            string frontendUrl = _configuration["VnPay:PaymentResultUrl"] ?? "http://localhost:5173/payment-result";

            // Xây dựng query string để chuyển thông tin giao dịch
            var queryString = $"?orderId={orderId}" +
                              $"&vnpayTranId={vnpayTranId}" +
                              $"&TerminalID={TerminalID}" +
                              $"&vnp_Amount={vnp_Amount}" +
                              $"&bankCode={bankCode}" +
                              $"&vnp_ResponseCode={vnp_ResponseCode}" +
                              $"&payDate={payDate}";

            // Nếu chữ ký hợp lệ, chuyển hướng về frontend với dữ liệu giao dịch
            if (checkSignature)
            {
                // Giao dịch thành công nếu ResponseCode và TransactionStatus đều là "00"
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                    return Redirect(frontendUrl + queryString);
                }
                else
                {
                    // Giao dịch không thành công, chuyển hướng với thông tin lỗi
                    return Redirect(frontendUrl + queryString);
                }
            }
            else
            {
                // Nếu chữ ký không hợp lệ, có thể chuyển hướng tới trang thông báo lỗi
                return Redirect(frontendUrl + "?error=InvalidSignature");
            }
            //    var result = await _orderService.HandlePaymentCallbackAsync(Request.Query);

            //    if (result.VnPayResponseCode == "00")
            //    {
            //        // Thanh toán thành công
            //        var successHtml = @"
            //    <html>
            //    <head>
            //        <style>
            //            body {
            //                margin: 0;
            //                background-color: #FFFFFF; /* Màu nền xanh nhạt */
            //                display: flex;
            //                justify-content: center;
            //                align-items: center;
            //                height: 100vh;
            //            }
            //            img {
            //                max-width: 80%;
            //                max-height: 80%;
            //                object-fit: contain;
            //            }
            //        </style>
            //    </head>
            //    <body>
            //        <img src='/images/payment-success.png' alt='Payment Successful' />
            //    </body>
            //    </html>
            //";
            //        return Content(successHtml, "text/html");
            //    }
            //    else
            //    {
            //        // Thanh toán thất bại
            //        var failureHtml = @"
            //    <html>
            //    <head>
            //        <style>
            //            body {
            //                margin: 0;
            //                background-color: #f8d7da; /* Màu nền đỏ nhạt */
            //                display: flex;
            //                justify-content: center;
            //                align-items: center;
            //                height: 100vh;
            //            }
            //            img {
            //                max-width: 80%;
            //                max-height: 80%;
            //                object-fit: contain;
            //            }
            //        </style>
            //    </head>
            //    <body>
            //        <img src='/images/payment-failure.png' alt='Payment Failed' />
            //    </body>
            //    </html>
            //";
            //        return Content(failureHtml, "text/html");
            //    }
        }
    }
}
