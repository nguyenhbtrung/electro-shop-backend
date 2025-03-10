using electro_shop_backend.Models.DTOs.Product;

namespace electro_shop_backend.Models.DTOs.Discount
{
    public class ProductGroupedDto
    {
        public IEnumerable<ProductDto> Available { get; set; } = new List<ProductDto>();
        public IEnumerable<ProductDto> Selected { get; set; } = new List<ProductDto>();
    }
}
