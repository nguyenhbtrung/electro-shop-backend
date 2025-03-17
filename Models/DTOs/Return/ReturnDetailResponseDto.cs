
namespace electro_shop_backend.Models.DTOs.Return
{
    public class ReturnDetailResponseDto
    {
        public int ReturnId { get; set; }
        public int? OrderId { get; set; }
        public string? Reason { get; set; }
        public string? Detail { get; set; }
        public string? Status { get; set; }
        public string? ReturnMethod { get; set; }
        public string? AdminComment { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<ReturnHistoryDto> ReturnHistories { get; set; } = [];
        public List<ReturnProductDto> ReturnProducts { get; set; } = [];
    }
}
