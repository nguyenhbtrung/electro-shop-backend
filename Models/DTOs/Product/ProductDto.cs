using electro_shop_backend.Models.DTOs.ProductImage;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.DTOs.Category;
using electro_shop_backend.Models.DTOs.Brand;
namespace electro_shop_backend.Models.DTOs.Product
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string? Info { get; set; }
        public decimal OriginalPrice { get; set; }

        public decimal DiscountedPrice { get; set; }
        public string? DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public double AverageRating { get; set; }

        public int Stock { get; set; }

        public List<ProductImageDto> ProductImages { get; set; } = new();
        public List<CategoryIdDto> Categories { get; set; } = new();
        public List<AttributeDetail> ProductAttributeDetail{ get; set; } = new();
        public BrandDto? Brand { get; set; }
    }
}
