namespace electro_shop_backend.Models.DTOs.Rating
{
    public class RatingDto
    {
        public int ProductId { get; set; }
        public string UserId { get; set; } = null!;
        public int RatingScore { get; set; }
        public string? RatingContent { get; set; }
        public string? Status { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
