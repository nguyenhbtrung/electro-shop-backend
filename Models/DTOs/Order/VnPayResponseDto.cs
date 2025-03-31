namespace electro_shop_backend.Models.DTOs.Order
{
    public class VnPayResponseDto
    {
        public bool Success { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderDescription { get; set; }
        public int OrderId { get; set; }
        public string PaymentId { get; set; }
        public string TransactionId { get; set; }
        public string? Txn_Ref { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }

    }
}
