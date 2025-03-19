namespace electro_shop_backend.Models.DTOs.Return
{
    public class AllReturnDto
    {
        public int ReturnId { get; set; }
        public int? OrderId { get; set; }
        public string? Status { get; set; }
        public string? ReturnMethod { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
