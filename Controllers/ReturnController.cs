using electro_shop_backend.Exceptions;
using electro_shop_backend.Extensions;
using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.Rating;
using electro_shop_backend.Models.DTOs.Return;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Services;
using electro_shop_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;

namespace electro_shop_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnController : ControllerBase
    {
        private readonly IReturnService _returnService;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public ReturnController(IReturnService returnService, UserManager<User> userManager, IConfiguration configuration)
        {
            _returnService = returnService;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllReturn()
        {
            try
            {
                var returns = await _returnService.GetAllReturnAsync();
                return Ok(returns);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{returnId}")]
        [Authorize]
        public async Task<IActionResult> GetReturnById([FromRoute] int returnId)
        {
            //var username = User.GetUsername();
            //var user = await _userManager.FindByNameAsync(username);
            //if (user == null)
            //{
            //    return NotFound();
            //}
            try
            {
                var result = await _returnService.GetReturnByIdAsync(returnId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("admin/{returnId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetReturnByAdmin([FromRoute] int returnId)
        {
            //var username = User.GetUsername();
            //var user = await _userManager.FindByNameAsync(username);
            //if (user == null)
            //{
            //    return NotFound();
            //}
            try
            {
                var result = await _returnService.GetReturnByAdminAsync(returnId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("history")]
        [Authorize]
        public async Task<IActionResult> GetUserReturnHistory()
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                var result = await _returnService.GetUserReturnHistoryAsync(user.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReturn([FromForm] CreateReturnRequestDto requestDto)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _returnService.CreateReturnAsync(user.Id, requestDto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{ReturnId}")]
        public async Task<IActionResult> UpdateReturn(int ReturnId, [FromBody] UpdateReturnDto requestDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _returnService.UpdateReturnAsync(ReturnId, requestDto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("status/{ReturnId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateReturnStatus(int ReturnId, [FromBody] UpdateReturnStatusRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _returnService.UpdateReturnStatusAsync(ReturnId, requestDto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{ReturnId}")]
        public async Task<IActionResult> DeleteReturn(int ReturnId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _returnService.DeleteReturnAsync(ReturnId);
            return Ok("Rating deleted successfully");
        }

        [HttpGet("payment/{orderId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetPaymentByOrderId([FromRoute] int orderId)
        {
            
            try
            {
                var result = await _returnService.GetPaymentByOrderIdAsync(orderId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Refund")]
        public async Task<IActionResult> RefundPayment([FromBody] RefundRequest refundRequest)
        {
            // Lấy thông tin cấu hình từ appsettings.json
            var vnp_Api = "https://sandbox.vnpayment.vn/merchant_webapi/api/transaction";           // Ví dụ: https://sandbox.vnpayment.vn/merchant_webapi/api/transactionRefund
            var vnp_HashSecret = _configuration["VnPay:HashSecret"]; // Secret Key của bạn
            var vnp_TmnCode = _configuration["VnPay:TmnCode"];       // Terminal Id

            // Khởi tạo các tham số hoàn tiền
            var vnp_RequestId = DateTime.Now.Ticks.ToString();     // Mã yêu cầu hoàn tiền duy nhất
            var vnp_Version = "2.1.0";                             // Phiên bản API
            var vnp_Command = "refund";
            var vnp_TransactionType = refundRequest.TransactionType; // Ví dụ: "FullRefund" hoặc giá trị khác theo VNPAY yêu cầu
            var vnp_Amount = refundRequest.Amount * 100;           // Quy đổi số tiền (nhân với 100)
            var vnp_TxnRef = refundRequest.TxnRef;                // Mã giao dịch tham chiếu cần hoàn tiền
            var vnp_OrderInfo = "Hoan tien giao dich:" + refundRequest.OrderId;
            var vnp_TransactionNo = "";                            // Nếu không có, để rỗng
            var vnp_TransactionDate = refundRequest.PayDate;       // Ngày giao dịch gốc (định dạng yyyyMMddHHmmss)
            var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var vnp_CreateBy = refundRequest.User;
            var vnp_IpAddr = HttpContext.Connection.RemoteIpAddress.ToString();

            // Tạo chuỗi dữ liệu cần ký theo thứ tự:
            // vnp_RequestId | vnp_Version | vnp_Command | vnp_TmnCode | vnp_TransactionType |
            // vnp_TxnRef | vnp_Amount | vnp_TransactionNo | vnp_TransactionDate | vnp_CreateBy |
            // vnp_CreateDate | vnp_IpAddr | vnp_OrderInfo
            var signData = string.Join("|", new string[] {
                vnp_RequestId,
                vnp_Version,
                vnp_Command,
                vnp_TmnCode,
                vnp_TransactionType,
                vnp_TxnRef,
                vnp_Amount.ToString(),
                vnp_TransactionNo,
                vnp_TransactionDate,
                vnp_CreateBy,
                vnp_CreateDate,
                vnp_IpAddr,
                vnp_OrderInfo
            });
            // Tính chữ ký HMAC SHA512
            var vnp_SecureHash = Utils.HmacSHA512(vnp_HashSecret, signData);

            // Tạo đối tượng dữ liệu gửi sang VNPAY
            var refundData = new
            {
                vnp_RequestId,
                vnp_Version,
                vnp_Command,
                vnp_TmnCode,
                vnp_TransactionType,
                vnp_TxnRef,
                vnp_Amount,
                vnp_OrderInfo,
                vnp_TransactionNo,
                vnp_TransactionDate,
                vnp_CreateBy,
                vnp_CreateDate,
                vnp_IpAddr,
                vnp_SecureHash
            };

            var jsonData = JsonConvert.SerializeObject(refundData);

            // Gửi yêu cầu POST sang VNPAY
            using (var client = new HttpClient())
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(vnp_Api, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                // Trả về kết quả từ VNPAY cho client
                return Ok(new { vnpayResponse = responseContent });
            }
        }
    }
}
