namespace electro_shop_backend.Models.DTOs.ProductViewHistory
{
    public class ProductViewHistoryDto
    {
        public int HistoryId { get; set; }
        public string? UserId { get; set; }
        public int? ProductId { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
