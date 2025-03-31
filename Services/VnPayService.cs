using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Helpers;
using electro_shop_backend.Models.DTOs.Order;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using electro_shop_backend.Models.DTOs.Order;

namespace electro_shop_backend.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VnPayService(ApplicationDbContext context, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        public string CreatePaymentUrl(VnPayRequestDto request)
        {
            var clientIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";

            var order = _context.Orders.FirstOrDefault(x => x.OrderId == request.OrderId);

            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", ((long)(order.Total*100)).ToString());
            vnpay.AddRequestData("vnp_TxnRef", $"{order.OrderId}_{DateTime.UtcNow:yyyyMMddHHmmssfff}");
            // Mã tham chiếu của giao dịch tại hệ thống của merchant.Mã này là duy nhất dùng để phân biệt các
            // đơn hàng gửi sang VNPAY.Không được trùng lặp trong ngày
            vnpay.AddRequestData("vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrencyCode"]);
            vnpay.AddRequestData("vnp_IpAddr", clientIp);
            vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:ReturnUrl"]);

            var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:Url"], _config["VnPay:HashSecret"]);
            return paymentUrl;
        }

        public VnPayResponseDto PaymentExcecute(IQueryCollection collection)
        {
            var vnpay = new VnPayLibrary();

            foreach(var(key, value) in collection)
            {
                if(!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_TxnRef = vnpay.GetResponseData("vnp_TxnRef");
            var vnp_orderId = int.Parse(vnp_TxnRef.Split('_')[0]);
            var vnp_transactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_secureHash = collection.FirstOrDefault(x => x.Key == "vnp_SecureHash").Value.ToString();
            var vnp_responseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_orderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = vnpay.ValidateSignature(vnp_secureHash, _config["VnPay:HashSecret"]);
            if(!checkSignature)
            {
                return new VnPayResponseDto
                {
                    Success = false,
                };
            }

            return new VnPayResponseDto
            {
                Success = true,
                OrderDescription = vnp_orderInfo,
                OrderId = (int)vnp_orderId,
                TransactionId = vnp_transactionId.ToString(),
                Token = vnpay.GetResponseData("vnp_SecureHash"),
                VnPayResponseCode = vnp_responseCode,
                Txn_Ref = vnp_TxnRef
            };
        }
    }
}
