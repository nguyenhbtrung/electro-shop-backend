namespace electro_shop_backend.Models.DTOs.Return
{
    public class ReturnProductDto
    {
        public int? ProductId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int? ReturnQuantity { get; set; }

    }
}
