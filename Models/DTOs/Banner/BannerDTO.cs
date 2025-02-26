namespace electro_shop_backend.Models.DTOs.Banner
{
    public class BannerDTO
    {
        public string? ImageUrl { get; set; }
        public string? Link { get; set; }
        public string? Title { get; set; }
        public int? Position { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
