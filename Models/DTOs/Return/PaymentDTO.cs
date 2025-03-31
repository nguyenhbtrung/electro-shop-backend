namespace electro_shop_backend.Models.DTOs.Return
{
    public class PaymentDTO
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string? PayDate { get; set; }
        public string? PaymentMethod { get; set; }
        public string? TxnRef { get; set; }

    }
}
