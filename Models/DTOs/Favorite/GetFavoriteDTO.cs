namespace electro_shop_backend.Models.DTOs.Favorite
{
    public class GetFavoriteDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

    }
}
