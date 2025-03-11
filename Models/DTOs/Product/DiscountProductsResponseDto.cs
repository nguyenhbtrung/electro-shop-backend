namespace electro_shop_backend.Models.DTOs.Product
{
    public class DiscountProductsResponseDto
    {
        public List<DiscountedProductDto> Products { get; set; } = new();
        public List<int?> SelectedProductIds { get; set; } = new();
    }
}
