namespace electro_shop_backend.Models.DTOs.Return
{
    public class ReturnUserHistoryDto
    {
        public int ReturnId { get; set; }
        public string? Status { get; set; }
        public string? ReturnMethod { get; set; }
        public DateTime? TimeStamp { get; set; }
        public List<ReturnProductDto> ReturnProducts { get; set; } = [];
    }
}
