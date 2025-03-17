namespace electro_shop_backend.Models.DTOs.Price
{
    public class PriceCalculationDto
    {
        public int ProductId { get; set; }
        public List<int> SelectedAttributeDetailIds { get; set; } = new List<int>();
    }
}
