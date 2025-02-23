namespace electro_shop_backend.Models.DTOs.ProductViewHistory
{
    public class CreateProductViewHistoryRequestDto
    {
        public string? UserId { get; set; }
        public int? ProductId { get; set; }
    }
}
