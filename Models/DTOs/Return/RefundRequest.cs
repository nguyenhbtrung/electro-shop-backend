namespace electro_shop_backend.Models.DTOs.Return
{
    public class RefundRequest
    {
        /// <summary>
        /// Số tiền hoàn (đơn vị VND, chưa nhân 100)
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Mã giao dịch thanh toán tham chiếu
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// Ngày giao dịch gốc (định dạng yyyyMMddHHmmss)
        /// </summary>
        public string PayDate { get; set; }

        /// <summary>
        /// Người tạo yêu cầu hoàn tiền
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Loại giao dịch hoàn tiền (ví dụ: "FullRefund" hoặc "PartialRefund")
        /// </summary>
        public string TransactionType { get; set; }
        public string TxnRef {  get; set; }
    }
}
